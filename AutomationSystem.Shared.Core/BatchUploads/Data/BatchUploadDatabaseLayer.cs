using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.BatchUploads.Data.Models;
using AutomationSystem.Shared.Core.BatchUploads.Data.Extensions;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;
using PerfectlyMadeInc.Helpers.Contract.Database;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Shared.Core.BatchUploads.Data
{
    /// <summary>
    /// Batch upload database layer
    /// </summary>
    public class BatchUploadDatabaseLayer : IBatchUploadDatabaseLayer
    {

        // gets batch uploads types (null = all)
        public List<BatchUploadType> GetBatchUploadTypes(IEnumerable<BatchUploadTypeEnum> ids = null)
        {
            using (var context = new CoreEntities())
            {
                IQueryable<BatchUploadType> query = context.BatchUploadTypes;
                if (ids != null)
                    query = query.Where(x => ids.Contains(x.BatchUploadTypeId));
                var result = query.ToList();
                return result;
            }
        }

        // gets batch upload operation types
        public List<BatchUploadOperationType> GetBatchUploadOperationTypes()
        {
            using (var context = new CoreEntities())
            {
                var result = context.BatchUploadOperationTypes.ToList();
                return result;
            }
        }


        // gets all batch uploads by parent        
        public List<BatchUpload> GetBatchUploadByParent(EntityTypeEnum? parentEntityTypeId, long? parentEntityId, BatchUploadIncludes includes = BatchUploadIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.BatchUploads.AddIncludes(includes).Active()
                    .Where(x => x.ParentEntityTypeId == parentEntityTypeId && x.ParentEntityId == parentEntityId).ToList();
                result.ForEach(x => BatchUploadRemoveInactive.RemoveInactiveForBatchUpload(x, includes));                    
                return result;
            }
        }


        // gets batch upload by id 
        public BatchUpload GetBatchUploadById(long batchUploadId, BatchUploadIncludes includes = BatchUploadIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.BatchUploads.AddIncludes(includes).Active().FirstOrDefault(x => x.BatchUploadId == batchUploadId);
                if (result != null)
                    BatchUploadRemoveInactive.RemoveInactiveForBatchUpload(result, includes);
                return result;
            }
        }


        // gets context keeper for batch upload by id
        public IDatabaseContextKeeper<BatchUpload> GetContextKeeperForBatchUploadById(long batchUploadId, BatchUploadIncludes includes = BatchUploadIncludes.None)
        {
            CoreEntities context = new CoreEntities();
            try
            {                
                var resultEntity = context.BatchUploads.AddIncludes(includes).Active().FirstOrDefault(x => x.BatchUploadId == batchUploadId);                

                var result = new DatabaseContextKeeper<BatchUpload>(context, resultEntity);
                return result;
            }
            catch (Exception)
            {
                context.Dispose();
                throw;
            }
        }


        // gets batch upload item by id
        public BatchUploadItem GetBatchUploadItemById(long batchUploadItemId, BatchUploadItemIncludes includes = BatchUploadItemIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.BatchUploadItems.AddIncludes(includes).Active().FirstOrDefault(x => x.BatchUploadItemId == batchUploadItemId);
                return result;
            }
        }

        // gets context keeper for batch upload item by id
        public IDatabaseContextKeeper<BatchUploadItem> GetContextKeeperForBatchUploadItemById(long batchUploadItemId,
            BatchUploadItemIncludes includes = BatchUploadItemIncludes.None)
        {
            CoreEntities context = new CoreEntities();
            try
            {
                var resultEntity = context.BatchUploadItems.AddIncludes(includes).Active().FirstOrDefault(x => x.BatchUploadItemId == batchUploadItemId);
                var result = new DatabaseContextKeeper<BatchUploadItem>(context, resultEntity);
                return result;
            }
            catch(Exception)
            {
                context.Dispose();
                throw;
            }
        }



        // inserts batch upload
        public long InsertBatchUpload(BatchUpload batchUpload)
        {
            using (var context = new CoreEntities())
            {
                context.BatchUploads.Add(batchUpload);
                context.SaveChanges();
                return batchUpload.BatchUploadId;
            }   
        }

        // deletes batch upload items
        public void DeleteInvalidBatchUploadItems(long bachUploadId)
        {
            using (var context = new CoreEntities())
            {
                var batchItemsToDelete = context.BatchUploadItems.Active().Where(x => x.BatchUploadId == bachUploadId && !x.IsValid).ToList();
                context.BatchUploadItems.RemoveRange(batchItemsToDelete);
                context.SaveChanges();
            }
           
        }


        // sets batch upload to discarded
        public long? SetBatchUploadToDiscarded(long batchUploadId, BatchUploadOperationOptions options = BatchUploadOperationOptions.None)
        {
            using (var context = new CoreEntities())
            {
                var toDiscard = context.BatchUploads.Active().FirstOrDefault(x => x.BatchUploadId == batchUploadId);
                if (toDiscard == null)
                    throw new ArgumentException($"There is no Batch upload with id {batchUploadId}.");

                if (toDiscard.IsProcessed && options.HasFlag(BatchUploadOperationOptions.CheckOperation))
                    throw new InvalidOperationException($"Batch upload with id {batchUploadId} is processed and cannot be discarded.");

                toDiscard.BatchUploadStateId = BatchUploadStateEnum.Discarded;
                context.SaveChanges();
                return toDiscard.ParentEntityId;
            }
        }

    }

}
