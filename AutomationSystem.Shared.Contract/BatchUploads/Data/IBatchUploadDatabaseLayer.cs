using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.BatchUploads.Data.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Contract.Database;

namespace AutomationSystem.Shared.Contract.BatchUploads.Data
{
    /// <summary>
    /// Batch upload database layer
    /// </summary>
    public interface IBatchUploadDatabaseLayer
    {

        // gets batch uploads types (null = all)
        List<BatchUploadType> GetBatchUploadTypes(IEnumerable<BatchUploadTypeEnum> ids = null);

        // gets batch upload operation types
        List<BatchUploadOperationType> GetBatchUploadOperationTypes();

        // gets all batch uploads by parent
        // todo: when new parameters will be required, convert to filter !!
        List<BatchUpload> GetBatchUploadByParent(EntityTypeEnum? parentEntityTypeId, long? parentEntityId, BatchUploadIncludes includes = BatchUploadIncludes.None);


        // gets batch upload by id 
        BatchUpload GetBatchUploadById(long batchUploadId, BatchUploadIncludes includes = BatchUploadIncludes.None);

        // gets context keeper for batch upload by id
        IDatabaseContextKeeper<BatchUpload> GetContextKeeperForBatchUploadById(long batchUploadId, BatchUploadIncludes includes = BatchUploadIncludes.None);


        // gets batch upload item by id
        BatchUploadItem GetBatchUploadItemById(long batchUploadItemId, BatchUploadItemIncludes includes = BatchUploadItemIncludes.None);

        // gets context keeper for batch upload item by id
        IDatabaseContextKeeper<BatchUploadItem> GetContextKeeperForBatchUploadItemById(long batchUploadItemId, BatchUploadItemIncludes includes = BatchUploadItemIncludes.None);


        // inserts batch upload
        long InsertBatchUpload(BatchUpload batchUpload);

        // deletes batch upload items
        void DeleteInvalidBatchUploadItems(long bachUploadId);

        // sets batch upload to discarded
        long? SetBatchUploadToDiscarded(long batchUploadId, BatchUploadOperationOptions options = BatchUploadOperationOptions.None);

    }

}
