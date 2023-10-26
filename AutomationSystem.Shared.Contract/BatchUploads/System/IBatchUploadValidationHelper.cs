using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.BatchUploads.System
{
    public interface IBatchUploadValidationHelper
    {
        void ValidateString(BatchUploadField field, string name, bool isRequired, int maxLength);

        void ValidateEmailString(BatchUploadField field, string name, bool isRequired, int maxLength);

        void ValidateDropDown(BatchUploadField field, string name, bool notSelected);
    }
}
