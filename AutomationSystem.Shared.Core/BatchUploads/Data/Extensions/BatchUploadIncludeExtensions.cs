using System.Data.Entity.Infrastructure;
using AutomationSystem.Shared.Contract.BatchUploads.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.BatchUploads.Data.Extensions
{
    /// <summary>
    /// Aggregates include extensions
    /// </summary>
    public static class BatchUploadIncludeExtensions
    {
        // add includes for BatchUpdate
        public static DbQuery<BatchUpload> AddIncludes(this DbQuery<BatchUpload> query, BatchUploadIncludes includes)
        {
            if (includes.HasFlag(BatchUploadIncludes.BatchUploadType))
                query = query.Include("BatchUploadType");
            if (includes.HasFlag(BatchUploadIncludes.BatchUploadState))
                query = query.Include("BatchUploadState");
            if (includes.HasFlag(BatchUploadIncludes.BatchUploadItems))
                query = query.Include("BatchUploadItems");
            if (includes.HasFlag(BatchUploadIncludes.BatchUploadItemsAndFields))
                query = query.Include("BatchUploadItems.BatchUploadFields");
            return query;
        }

        // add includes for BatchUploadItem
        public static DbQuery<BatchUploadItem> AddIncludes(this DbQuery<BatchUploadItem> query, BatchUploadItemIncludes includes)
        {
            if (includes.HasFlag(BatchUploadItemIncludes.BatchUploadFields))
            {
                query = query.Include("BatchUploadFields");
            }

            if (includes.HasFlag(BatchUploadItemIncludes.BatchUploadOperationType))
            {
                query = query.Include("BatchUploadOperationType");
            }

            if (includes.HasFlag(BatchUploadItemIncludes.BatchUpload))
            {
                query = query.Include("BatchUpload");
            }

            return query;
        }
    }
}
