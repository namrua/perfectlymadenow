using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.RegistrationUpload.AppLogic
{
    public interface IStudentRegistrationBatchMapper
    {
        BatchUploadDetail<RegistrationStudentDetail> MapToBatchUploadDetail(BatchUpload batchUpload);

        RegistrationStudentForm MapToStudentForm(BatchUploadItem item);
        
        void UpdateBatchUploadItem(RegistrationStudentForm form, BatchUploadItem item);
    }
}
