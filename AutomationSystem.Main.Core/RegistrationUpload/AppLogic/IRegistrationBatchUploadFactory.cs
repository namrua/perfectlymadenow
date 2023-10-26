using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Shared.Model;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.RegistrationUpload.AppLogic
{
    public interface IRegistrationBatchUploadFactory
    {
        RegistrationStudentBatchItemForEdit CreateStudentBatchItemForEdit(BatchUploadItem batchUploadItem);

        RegistrationBatchUploadForEdit CreateRegistrationBatchUploadForEdit(ICollection<BatchUploadTypeEnum> batchUploadTypes, long classId);
    }
}
