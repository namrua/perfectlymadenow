using System;

namespace AutomationSystem.Shared.Contract.BatchUploads.Data.Models
{
    /// <summary>
    /// Batch upload includes
    /// </summary>
    [Flags]
    public enum BatchUploadIncludes
    {
        None = 0x00,
        BatchUploadType = 0x01,
        BatchUploadState = 0x02,
        BatchUploadItems = 0x04,
        BatchUploadItemsAndFields = 0x08,
    }

}
