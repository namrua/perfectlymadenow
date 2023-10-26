using System;

namespace AutomationSystem.Shared.Contract.BatchUploads.Data.Models
{
    /// <summary>
    /// Batch upload operation options
    /// </summary>
    [Flags]
    public enum BatchUploadOperationOptions
    {
        None = 0x00,
        CheckOperation = 0x01,
    }

}
