using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Contract.RegistrationUpload.AppLogic;
using AutomationSystem.Main.Contract.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic.BatchDataFetchers;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.BatchUploads.Data.Models;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomationSystem.Main.Core.RegistrationUpload.AppLogic
{
    public class RegistrationBatchUploadAdministration : IRegistrationBatchUploadAdministration
    {

        private readonly IBatchUploadDatabaseLayer batchUploadDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly ICoreFileService coreFileService;
        private readonly IStudentRegistrationBatchValidator batchValidator;
        private readonly IStudentRegistrationBatchMapper studentBatchMapper;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IBatchUploadMergeResolver<ClassRegistration> registrationMergeResolver;
        private readonly IRegistrationPersonalDataService service;
        private readonly IRegistrationCommandService commandService;
        private readonly IStudentRegistrationBatchUploadValueResolver batchUploadValueResolver;
        private readonly IRegistrationBatchUploadFactory studentRegistrationFactory;
        private readonly IBatchUploadFactory batchUploadFactory;
        private readonly IIdentityResolver identityResolver;
        private readonly IClassOperationChecker classOperationChecker;

        private readonly Dictionary<BatchUploadTypeEnum, IStudentRegistrationBatchDataFileFetcher> fetchers;


        public RegistrationBatchUploadAdministration(
            IBatchUploadDatabaseLayer batchUploadDb,
            IClassDatabaseLayer classDb,
            ICoreFileService coreFileService,
            IStudentRegistrationBatchValidator batchValidator,
            IStudentRegistrationBatchMapper studentBatchMapper,
            IRegistrationDatabaseLayer registrationDb,
            IBatchUploadMergeResolver<ClassRegistration> registrationMergeResolver,
            IEnumerable<IStudentRegistrationBatchDataFileFetcher> batchFileDataFetchers,
            IRegistrationPersonalDataService service,
            IRegistrationCommandService commandService,
            IStudentRegistrationBatchUploadValueResolver batchUploadValueResolver,
            IRegistrationBatchUploadFactory studentRegistrationFactory,
            IBatchUploadFactory batchUploadFactory,
            IIdentityResolver identityResolver,
            IClassOperationChecker classOperationChecker)
        {
            this.batchUploadDb = batchUploadDb;
            this.classDb = classDb;
            this.coreFileService = coreFileService;
            this.batchValidator = batchValidator;
            this.studentBatchMapper = studentBatchMapper;
            this.registrationDb = registrationDb;
            this.registrationMergeResolver = registrationMergeResolver;
            this.service = service;
            this.commandService = commandService;
            this.batchUploadValueResolver = batchUploadValueResolver;
            this.studentRegistrationFactory = studentRegistrationFactory;
            this.batchUploadFactory = batchUploadFactory;
            this.identityResolver = identityResolver;
            this.classOperationChecker = classOperationChecker;

            fetchers = batchFileDataFetchers.ToDictionary(x => x.BatchUploadTypeId);
        }

        public BatchUploadDetail<RegistrationStudentDetail> GetBatchUploadDetail(long batchUploadId)
        {
            var batchUpload = batchUploadDb.GetBatchUploadById(batchUploadId, BatchUploadIncludes.BatchUploadItemsAndFields
                                                                              | BatchUploadIncludes.BatchUploadState | BatchUploadIncludes.BatchUploadType);
            if (batchUpload == null)
            {
                throw new ArgumentException($"There is no Batch upload with id {batchUploadId}.");
            }

            CheckIdentityAndOperation(batchUpload.ParentEntityId ?? 0);

            // converts batch upload to detail
            var result = studentBatchMapper.MapToBatchUploadDetail(batchUpload);
            return result;
        }

        public RegistrationBatchUploadForEdit GetNewBatchUploadForEdit(long classId)
        {
            CheckIdentityAndOperation(classId);

            var result = studentRegistrationFactory.CreateRegistrationBatchUploadForEdit(fetchers.Keys, classId);
            result.Form.ParentEntityId = classId;
            return result;
        }

        public RegistrationBatchUploadForEdit GetBatchUploadForEditByForm(RegistrationBatchUploadForm form)
        {
            var result = studentRegistrationFactory.CreateRegistrationBatchUploadForEdit(fetchers.Keys, form.ParentEntityId);
            result.Form = form;
            return result;
        }

        public BatchUploadResult UploadBatch(RegistrationBatchUploadForm form, Stream batchFile, string fileName)
        {
            CheckIdentityAndOperation(form.ParentEntityId);

            var result = new BatchUploadResult();
            var dataFetcher = GetDataFetcher(form.BatchUploadTypeId);
            
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

            var parameter = new RegistrationBatchUploadParameters
            {
                RegistrationTypeId = form.RegistrationTypeId
            };

            // convert, validates
            var batchUpload = batchUploadFactory.CreateBatchUpload(rawData, form, EntityTypeEnum.MainClass, EntityTypeEnum.MainClassRegistration, batchUploadValueResolver, parameter);
            ValidateBatch(batchUpload);

            // saves batch
            batchUpload.FileId = coreFileService.InsertFile(fileContent, batchUpload.Name, fileName, dataFetcher.BatchFileTypeId);
            var batchUploadId = batchUploadDb.InsertBatchUpload(batchUpload);

            result.IsSuccess = true;
            result.BatchUploadId = batchUploadId;
            return result;
        }

        public RegistrationStudentBatchItemForEdit GetStudentBatchItemForEdit(long batchUploadItemId)
        {
            var item = batchUploadDb.GetBatchUploadItemById(batchUploadItemId, BatchUploadItemIncludes.BatchUploadFields | BatchUploadItemIncludes.BatchUpload);
            if (item == null)
            {
                throw new ArgumentException($"There is no Batch upload item with id {batchUploadItemId}");
            }

            CheckIdentityAndOperation(item.BatchUpload.ParentEntityId ?? 0);

            var result = studentRegistrationFactory.CreateStudentBatchItemForEdit(item);
            result.Form = studentBatchMapper.MapToStudentForm(item);
            return result;
        }

        public RegistrationStudentBatchItemForEdit GetStudentBatchItemForEditByForm(RegistrationStudentForm form, long batchUploadItemId)
        {
            var item = batchUploadDb.GetBatchUploadItemById(batchUploadItemId, BatchUploadItemIncludes.BatchUploadFields);
            if (item == null)
            {
                throw new ArgumentException($"There is no Batch upload item with id {batchUploadItemId}");
            }

            var result = studentRegistrationFactory.CreateStudentBatchItemForEdit(item);
            result.Form = form;
            return result;
        }

        public long SaveBatchUploadItem(RegistrationStudentForm form, long batchUploadItemId)
        {
            long result;

            using (var context = batchUploadDb.GetContextKeeperForBatchUploadItemById(batchUploadItemId, BatchUploadItemIncludes.BatchUploadFields | BatchUploadItemIncludes.BatchUpload))
            {
                if (context.Result == null)
                {
                    throw new ArgumentException($"There is no Batch upload item with id {batchUploadItemId}");
                }

                CheckIdentityAndOperation(context.Result.BatchUpload.ParentEntityId ?? 0);

                result = context.Result.BatchUploadId;
                studentBatchMapper.UpdateBatchUploadItem(form, context.Result);
                context.SaveChanges();
            }
            return result;
        }

        public BatchUploadStateEnum CompleteValidation(long batchUploadId)
        {
            batchUploadDb.DeleteInvalidBatchUploadItems(batchUploadId);

            using (var context = batchUploadDb.GetContextKeeperForBatchUploadById(batchUploadId, BatchUploadIncludes.BatchUploadItemsAndFields))
            {
                var batchUpload = context.Result;
                CheckIdentityAndOperation(batchUpload.ParentEntityId ?? 0);

                var parameter = JsonConvert.DeserializeObject<RegistrationBatchUploadParameters>(batchUpload.JsonParameters);
                var filter = new RegistrationFilter
                {
                    ClassId = batchUpload.ParentEntityId,
                    RegistrationTypeIdsEnum = new List<RegistrationTypeEnum>
                    {
                        parameter.RegistrationTypeId
                    }
                };
                var registrations = registrationDb.GetRegistrationsByFilter(filter, ClassRegistrationIncludes.Addresses);

                var isMerged = MergeBatch(batchUpload, registrations);
                if (isMerged)
                {
                    ProcessBatch(batchUpload);
                }

                context.SaveChanges();
                return batchUpload.BatchUploadStateId;
            }
        }

        public BatchUploadStateEnum CompleteMerging(long batchUploadId, Dictionary<long, BatchUploadOperationTypeEnum> operationMap)
        {
            using (var context = batchUploadDb.GetContextKeeperForBatchUploadById(batchUploadId, BatchUploadIncludes.BatchUploadItemsAndFields))
            {
                var batchUpload = context.Result;
                CheckIdentityAndOperation(batchUpload.ParentEntityId ?? 0);

                foreach (var  item in batchUpload.BatchUploadItems.AsQueryable().Active())
                {
                    if (!operationMap.TryGetValue(item.BatchUploadItemId, out var operationTypeId))
                    {
                        continue;
                    }

                    if (!item.PairEntityId.HasValue && operationTypeId == BatchUploadOperationTypeEnum.Update)
                    {
                        throw new InvalidOperationException($"Operation {operationTypeId} is not allowed for Batch upload item with id {item.BatchUploadItemId}.");
                    }

                    item.BatchUploadOperationTypeId = operationTypeId;
                }

                ProcessBatch(batchUpload);
                context.SaveChanges();
                return batchUpload.BatchUploadStateId;
            }
        }


        public long? DiscardBatch(long batchUploadId)
        {
            var batchUpload = batchUploadDb.GetBatchUploadById(batchUploadId);
            CheckIdentityAndOperation(batchUpload.ParentEntityId ?? 0);

            var result = batchUploadDb.SetBatchUploadToDiscarded(batchUploadId, BatchUploadOperationOptions.CheckOperation);
            return result;
        }

        #region private methods

        private IBatchFileDataFetcher GetDataFetcher(BatchUploadTypeEnum? batchTypeId)
        {
            if (!batchTypeId.HasValue)
            {
                throw new ArgumentException("Batch type is empty");
            }

            if (!fetchers.TryGetValue(batchTypeId.Value, out var result))
            {
                throw new ArgumentException($"Unsupported batch type {batchTypeId}.");
            }

            return result;
        }

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

        private void ProcessBatch(BatchUpload batchUpload)
        {
            var cls = GetClassById(batchUpload.ParentEntityId ?? 0);
            var parameter = JsonConvert.DeserializeObject<RegistrationBatchUploadParameters>(batchUpload.JsonParameters);
            foreach (var item in batchUpload.BatchUploadItems.AsQueryable().Active().Where(x => !x.IsProcessed))
            {
                switch (item.BatchUploadOperationTypeId)
                {
                    case BatchUploadOperationTypeEnum.New:
                        var classRegistrationId = SaveAndApproveClassRegistration(item, parameter, cls.ClassId, cls.OriginLanguageId);
                        item.EntityId = classRegistrationId;
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

        private long SaveAndApproveClassRegistration(BatchUploadItem item, RegistrationBatchUploadParameters parameters, long classId, LanguageEnum languageId)
        {
            var form = studentBatchMapper.MapToStudentForm(item);
            form.ClassId = classId;
            form.LanguageId = languageId;
            form.RegistrationTypeId = parameters.RegistrationTypeId;
            var classRegistrationId = service.SaveRegistration(form);
            commandService.ApproveRegistration(classRegistrationId);

            return classRegistrationId;
        }

        private bool MergeBatch(BatchUpload batchUpload, IEnumerable<ClassRegistration> registrations)
        {
            var isMergeComplete = true;
            registrationMergeResolver.BindEntities(registrations);

            batchUpload.BatchUploadStateId = BatchUploadStateEnum.InMerging;
            foreach (var item in batchUpload.BatchUploadItems.AsQueryable().Active())
            {
                var pairRegistrationId = registrationMergeResolver.TryPair(item);
                if (pairRegistrationId.HasValue)
                {
                    item.PairEntityId = pairRegistrationId;
                    item.BatchUploadOperationTypeId = BatchUploadOperationTypeEnum.Ignore;
                    isMergeComplete = false;
                }
                else
                {
                    item.BatchUploadOperationTypeId = BatchUploadOperationTypeEnum.New;
                }
            }

            return isMergeComplete;
        }

        private Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var cls = classDb.GetClassById(classId, includes);
            if (cls == null)
            {
                throw new ArgumentException($"There is no class with id {classId}.");
            }

            return cls;
        }

        private void CheckIdentityAndOperation(long classId)
        {
            var cls = GetClassById(classId);
            identityResolver.CheckEntitleForClass(cls);
            classOperationChecker.CheckOperation(ClassOperation.AddRegistration, cls);
        }

        #endregion
    }
}
