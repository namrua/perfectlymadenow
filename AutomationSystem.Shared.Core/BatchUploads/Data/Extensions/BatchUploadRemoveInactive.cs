using System.Linq;
using AutomationSystem.Shared.Contract.BatchUploads.Data.Models;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.BatchUploads.Data.Extensions
{
    /// <summary>
    /// Aggregates include extensions
    /// </summary>
    public static class BatchUploadRemoveInactive
    {
        // removes inactive includes for BatchUpload
        public static BatchUpload RemoveInactiveForBatchUpload(BatchUpload entity, BatchUploadIncludes includes)
        {

            if (includes.HasFlag(BatchUploadIncludes.BatchUploadItems)
                || includes.HasFlag(BatchUploadIncludes.BatchUploadItemsAndFields))
            {
                entity.BatchUploadItems = entity.BatchUploadItems.AsQueryable().Active().ToList();
                if (includes.HasFlag(BatchUploadIncludes.BatchUploadItemsAndFields))
                    foreach (var batchUploadItem in entity.BatchUploadItems)
                        batchUploadItem.BatchUploadFields = batchUploadItem.BatchUploadFields.AsQueryable().Active().ToList();
            }
            return entity;
        }
    }
}
