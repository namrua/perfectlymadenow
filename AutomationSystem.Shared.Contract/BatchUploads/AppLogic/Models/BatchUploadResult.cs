namespace AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models
{
    /// <summary>
    /// Encapsulates result of batch upload
    /// </summary>
    public class BatchUploadResult
    {

        public bool IsSuccess { get; set; }
        public long? BatchUploadId { get; set; }
        public string ErrorMessage { get; set; }

    }
    
}
