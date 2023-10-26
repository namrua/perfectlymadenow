using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.BatchUploads.System
{
    /// <summary>
    /// Validates batch uploads
    /// </summary>
    public interface IBatchUploadValidator
    {

        // validates batch files and writes results in it
        bool Validate(BatchUploadItem uploadItem);

    }
    
}
