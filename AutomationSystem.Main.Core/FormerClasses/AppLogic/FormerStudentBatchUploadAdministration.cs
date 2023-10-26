using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Core.FormerClasses.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.BatchUploads.Data.Models;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.BatchDataFetchers;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{

    /// <summary>
    /// Batch upload service
    /// </summary>
    public class FormerStudentBatchUploadAdministration : IFormerStudentBatchUploadAdministration
    {
        
        private readonly IBatchUploadDatabaseLayer batchUploadDb;
        private readonly IFormerDatabaseLayer formerDb;
        private readonly ICoreFileService coreFileService;
        private readonly IFormerStudentUploadValidator batchValidator;
        private readonly IFormerStudentBatchConvertor formerStudentBatchConvertor;
        private readonly IBatchUploadService batchUploadService;
        private readonly IFormerStudentBatchUploadValueResolver batchUploadValueResolver;
        private readonly IBatchUploadFactory batchUploadFactory;
        private readonly IFormerStudentBatchUploadFactory formerStudentFactory;

        private readonly Dictionary<BatchUploadTypeEnum, IFormerStudentBatchFileDataFetcher> fetchers;

        // constructors
        public FormerStudentBatchUploadAdministration(
            IBatchUploadDatabaseLayer batchUploadDb,
            IFormerDatabaseLayer formerDb,
            ICoreFileService coreFileService,
            IFormerStudentUploadValidator batchValidator,
            IFormerStudentBatchConvertor formerStudentBatchConvertor,
            IEnumerable<IFormerStudentBatchFileDataFetcher> batchFileDataFetchers,
            IBatchUploadService batchUploadService,
            IFormerStudentBatchUploadValueResolver batchUploadValueResolver,
            IBatchUploadFactory batchUploadFactory,
            IFormerStudentBatchUploadFactory formerStudentFactory)
        {
            this.batchUploadDb = batchUploadDb;
            this.formerDb = formerDb;
            this.coreFileService = coreFileService;
            this.batchValidator = batchValidator;
            this.formerStudentBatchConvertor = formerStudentBatchConvertor;
            this.batchUploadService = batchUploadService;
            this.batchUploadValueResolver = batchUploadValueResolver;
            this.batchUploadFactory = batchUploadFactory;
            this.formerStudentFactory = formerStudentFactory;
            
            fetchers = batchFileDataFetchers.ToDictionary(x => x.BatchUploadTypeId);
        }



        // gets FormerStudentUploadPageModel
        public FormerStudentUploadPageModel GetFormerStudentUploadPageModel(long formerClassId)
        {
            var result = new FormerStudentUploadPageModel
            {
                FormerClassId = formerClassId,
                Items = batchUploadService.GetBatchUploadListItems(EntityTypeEnum.MainFormerClass, formerClassId)
            };
            return result;
        }


        // gets batch upload detail
        public BatchUploadDetail<FormerStudentDetail> GetBatchUploadDetail(long batchUploadId)
        {
            var batchUpload = batchUploadDb.GetBatchUploadById(batchUploadId, BatchUploadIncludes.BatchUploadItemsAndFields
                                                                              | BatchUploadIncludes.BatchUploadState | BatchUploadIncludes.BatchUploadType);
            if (batchUpload == null)
                throw new ArgumentException($"There is no Batch upload with id {batchUploadId}.");

            // converts batch upload to detail
            var result = formerStudentBatchConvertor.ConvertToBatchUploadDetail(batchUpload);
            return result;
        }


        // Gets new batch upload model
        public BatchUploadForEdit<BatchUploadForm> GetNewBatchUploadForEdit(long classId)
        {
            var result = new BatchUploadForEdit<BatchUploadForm>();
            result.Form.ParentEntityId = classId;
            result.FileTypes = batchUploadDb.GetBatchUploadTypes(fetchers.Keys).ToList();            
            return result;       
        }

        // gets batch upload for edit by form
        public BatchUploadForEdit<BatchUploadForm> GetFormBatchUploadForEdit(BatchUploadForm form)
        {            
            var result = new BatchUploadForEdit<BatchUploadForm>();
            result.FileTypes = batchUploadDb.GetBatchUploadTypes(fetchers.Keys).ToList();
            result.Form = form;
            return result;
        }              


        // uploads batch file
        public BatchUploadResult UploadBatch(BatchUploadForm form, Stream batchFile, string fileName)
        {
            var result = new BatchUploadResult();
            var dataFetcher = GetDataFetcher(form.BatchUploadTypeId);

            // checks that former class exists
            var formerClass = formerDb.GetFormerClassById(form.ParentEntityId);
            if (formerClass == null)
                throw new ArgumentException($"There is no Former class by id {form.ParentEntityId}.");

            // gets data
            List<string[]> rawData;
            var fileContent = coreFileService.ConvertStreamToByteArray(batchFile, seekToBegin: true);
            try
            {
                rawData = dataFetcher.FetchData(batchFile);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return result;
            }

            // convert, validates
            var batchUpload = batchUploadFactory.CreateBatchUpload(rawData, form, EntityTypeEnum.MainFormerClass, EntityTypeEnum.MainFormerStudent, batchUploadValueResolver);
            ValidateBatch(batchUpload);            

            // saves batch
            batchUpload.FileId = coreFileService.InsertFile(fileContent, batchUpload.Name, fileName, dataFetcher.BatchFileTypeId);
            var batchUploadId = batchUploadDb.InsertBatchUpload(batchUpload);

            result.IsSuccess = true;
            result.BatchUploadId = batchUploadId;
            return result;
        }

       


        // gets FormerStudentBatchItemForEdit by item id
        public FormerStudentBatchItemForEdit GetFormerStudentBatchItemForEdit(long batchUploadItemId)
        {
            var item = batchUploadDb.GetBatchUploadItemById(batchUploadItemId, BatchUploadItemIncludes.BatchUploadFields);
            if (item == null)
                throw new ArgumentException($"There is no Batch upload item with id {batchUploadItemId}");

            var result = formerStudentFactory.CreateFormerStudentBatchItemForEdit(item);
            result.Form = formerStudentBatchConvertor.ConvertToFormerStudentForm(item);
            return result;
        }

        // gets FormerStudentBatchItemForEdit by item id
        public FormerStudentBatchItemForEdit GetFormFormerStudentBatchItemForEdit(FormerStudentForm form, long batchUploadItemId)
        {
            var item = batchUploadDb.GetBatchUploadItemById(batchUploadItemId, BatchUploadItemIncludes.BatchUploadFields);
            if (item == null)
                throw new ArgumentException($"There is no Batch upload item with id {batchUploadItemId}");

            var result = formerStudentFactory.CreateFormerStudentBatchItemForEdit(item);
            result.Form = form;
            return result;
        }

        // save BatchUploadItem, returns BatchUpload ID
        public long SaveBatchUploadItem(FormerStudentForm form, long batchUploadItemId)
        {
            long result;
            using (var context = batchUploadDb.GetContextKeeperForBatchUploadItemById(batchUploadItemId, BatchUploadItemIncludes.BatchUploadFields))
            {
                if (context.Result == null) throw new ArgumentException($"There is no Batch upload item with id {batchUploadItemId}");

                result = context.Result.BatchUploadId;
                formerStudentBatchConvertor.UpdateBatchUploadItem(form, context.Result);
                context.SaveChanges();
            }
            return result;
        }


        // completes validation        
        public BatchUploadStateEnum CompleteValidation(long batchUploadId)
        {
            // deletes invalid batch items
            batchUploadDb.DeleteInvalidBatchUploadItems(batchUploadId);

            // batch operation block            
            using (var context = batchUploadDb.GetContextKeeperForBatchUploadById(batchUploadId, BatchUploadIncludes.BatchUploadItemsAndFields))
            {
                // loads former students
                var batchUpload = context.Result;
                var filter = new FormerStudentFilter {FormerClassId = batchUpload.ParentEntityId};
                var formerStudents = formerDb.GetFormerStudentsByFilter(filter, FormerStudentIncludes.Address);

                // executes merging of batch with former students
                var isMerged = MergeBatch(batchUpload, formerStudents);

                // complete process when merging is complete
                if (isMerged)
                    ProcessBatch(batchUpload);

                // saves changes and returns current status
                context.SaveChanges();
                return batchUpload.BatchUploadStateId;
            }
        }


        // completes merging
        public BatchUploadStateEnum CompleteMerging(long batchUploadId, Dictionary<long, BatchUploadOperationTypeEnum> operationMap)
        {
            // batch operation block            
            using (var context = batchUploadDb.GetContextKeeperForBatchUploadById(batchUploadId, BatchUploadIncludes.BatchUploadItemsAndFields))
            {
                // sets operation types
                var batchUpload = context.Result;
                foreach (var item in batchUpload.BatchUploadItems.AsQueryable().Active())
                {
                    if (!operationMap.TryGetValue(item.BatchUploadItemId, out var operationTypeId))
                        continue;
                    if (!item.PairEntityId.HasValue && operationTypeId == BatchUploadOperationTypeEnum.Update)
                        throw new InvalidOperationException($"Operation {operationTypeId} is not allowed for Batch upload item with id {item.BatchUploadItemId}.");
                    item.BatchUploadOperationTypeId = operationTypeId;
                }

                // complete processing of batch is complete              
                ProcessBatch(batchUpload);              
                context.SaveChanges();
                return batchUpload.BatchUploadStateId;
            }
        }


        // discards batch upload, returns parent entity id
        public long? Discard(long id)
        {
            var result = batchUploadDb.SetBatchUploadToDiscarded(id, BatchUploadOperationOptions.CheckOperation);
            return result;
        }


        #region private methods

        // gets data fetcher
        private IBatchFileDataFetcher GetDataFetcher(BatchUploadTypeEnum? batchTypeId)
        {
            if (!batchTypeId.HasValue)
                throw new ArgumentException("Batch type is empty");
            if (!fetchers.TryGetValue(batchTypeId.Value, out var result))
                throw new ArgumentException($"Unsupported batch type {batchTypeId}.");
            return result;
        }

        // validates batch
        private bool ValidateBatch(BatchUpload batchUpload)
        {
            var isValid = true;
            batchUpload.BatchUploadStateId = BatchUploadStateEnum.InValidation;
            foreach (var batchUploadItem in batchUpload.BatchUploadItems)
            {
                var isItemValid = batchValidator.Validate(batchUploadItem);
                isValid = isValid && isItemValid;
            }
            return isValid;
        }


        // merge data
        private bool MergeBatch(BatchUpload batchUpload, IEnumerable<FormerStudent> formerStudents)
        {
            var isMergeComplete = true;            
            var mergeResolver = new FormerStudentBatchMergeResolver();
            mergeResolver.BindEntities(formerStudents);

            batchUpload.BatchUploadStateId = BatchUploadStateEnum.InMerging;
            foreach (var item in batchUpload.BatchUploadItems.AsQueryable().Active())
            {
                var pairStudentId = mergeResolver.TryPair(item);
                if (pairStudentId.HasValue)
                {
                    item.PairEntityId = pairStudentId;
                    item.BatchUploadOperationTypeId = BatchUploadOperationTypeEnum.Ignore;
                    isMergeComplete = false;
                }
                else 
                    item.BatchUploadOperationTypeId = BatchUploadOperationTypeEnum.New;
            }
            return isMergeComplete;
        }


        // processes upload
        private void ProcessBatch(BatchUpload batchUpload)
        {            
            foreach (var item in batchUpload.BatchUploadItems.AsQueryable().Active().Where(x => !x.IsProcessed))
            {               
                var formerStudent = formerStudentBatchConvertor.ConvertToFormerStudent(batchUpload.ParentEntityId ?? 0, item);
                switch (item.BatchUploadOperationTypeId)
                {                   
                    case BatchUploadOperationTypeEnum.New:    
                        // todo: #OWNER
                        formerStudent.OwnerId = 1;
                        item.EntityId = formerDb.InsertFormerStudent(formerStudent);                                   
                        break;

                    case BatchUploadOperationTypeEnum.Update:
                        formerStudent.FormerStudentId = item.PairEntityId ?? 0;
                        item.EntityId = formerStudent.FormerStudentId;
                        formerDb.UpdateFormerStudent(formerStudent, FormerOperationOption.KeepOwnerId);                        
                        break;

                    case BatchUploadOperationTypeEnum.Ignore:
                        break;
                        
                    default:
                        continue;
                }
                item.IsProcessed = true;
                item.Processed = DateTime.Now;
            }
            batchUpload.BatchUploadStateId = BatchUploadStateEnum.Complete;
            batchUpload.IsProcessed = true;
            batchUpload.Processed = DateTime.Now;
        }       

        #endregion        

    }

}
