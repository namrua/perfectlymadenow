using System.Collections.Generic;
using System.IO;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic
{
    /// <summary>
    /// Batch upload service
    /// </summary>
    public interface IFormerStudentBatchUploadAdministration
    {

        // gets FormerStudentUploadPageModel
        FormerStudentUploadPageModel GetFormerStudentUploadPageModel(long formerClassId);

        // gets batch upload detail
        BatchUploadDetail<FormerStudentDetail> GetBatchUploadDetail(long batchUploadId);



        // gets new batch upload model
        BatchUploadForEdit<BatchUploadForm> GetNewBatchUploadForEdit(long classId);

        // gets batch upload for edit by form
        BatchUploadForEdit<BatchUploadForm> GetFormBatchUploadForEdit(BatchUploadForm form);

        // uploads batch file
        BatchUploadResult UploadBatch(BatchUploadForm form, Stream batchFile, string fileName);
                
       

        // gets FormerStudentBatchItemForEdit by item id
        FormerStudentBatchItemForEdit GetFormerStudentBatchItemForEdit(long batchUploadItemId);

        // gets FormerStudentBatchItemForEdit by item id
        FormerStudentBatchItemForEdit GetFormFormerStudentBatchItemForEdit(FormerStudentForm form, long batchUploadItemId);

        // save BatchUploadItem, returns BatchUpload ID
        long SaveBatchUploadItem(FormerStudentForm form, long batchUploadItemId);


        // completes validation
        BatchUploadStateEnum CompleteValidation(long batchUploadId);

        // completes merging
        BatchUploadStateEnum CompleteMerging(long batchUploadId, Dictionary<long, BatchUploadOperationTypeEnum> operationMap);


        // discards batch upload, returns parent entity id
        long? Discard(long id);

    }   

}