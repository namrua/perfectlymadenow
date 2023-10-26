using System;

namespace AutomationSystem.Shared.Contract.BatchUploads.Data.Models
{
    /// <summary>
    /// Batch upload item includes
    /// </summary>
    [Flags]
    public enum BatchUploadItemIncludes
    {
        None = 0,
        BatchUploadFields = 1 << 0,
        BatchUploadOperationType = 1 << 1,
        BatchUpload = 1 << 2 
    }

}
