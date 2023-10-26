using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.RegistrationUpload.AppLogic.Models;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using System.Collections.Generic;
using System.IO;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;

namespace AutomationSystem.Main.Contract.RegistrationUpload.AppLogic
{
    public interface IRegistrationBatchUploadAdministration
    {
        BatchUploadDetail<RegistrationStudentDetail> GetBatchUploadDetail(long batchUploadId);

        RegistrationBatchUploadForEdit GetNewBatchUploadForEdit(long classId);

        RegistrationBatchUploadForEdit GetBatchUploadForEditByForm(RegistrationBatchUploadForm form);
        
        BatchUploadResult UploadBatch(RegistrationBatchUploadForm form, Stream batchFile, string fileName);

        RegistrationStudentBatchItemForEdit GetStudentBatchItemForEdit(long batchUploadItemId);

        RegistrationStudentBatchItemForEdit GetStudentBatchItemForEditByForm(RegistrationStudentForm form, long batchUploadItemId);

        long SaveBatchUploadItem(RegistrationStudentForm form, long batchUploadItemId);

        BatchUploadStateEnum CompleteValidation(long batchUploadId);
        
        BatchUploadStateEnum CompleteMerging(long batchUploadId, Dictionary<long, BatchUploadOperationTypeEnum> operationMap);

        long? DiscardBatch(long batchUploadId);
    }
}
