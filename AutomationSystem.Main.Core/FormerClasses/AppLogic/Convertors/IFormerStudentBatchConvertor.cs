using System.Collections.Generic;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors
{
    /// <summary>
    /// Former student batch convertor
    /// </summary>
    public interface IFormerStudentBatchConvertor
    {
        // converts batch item to former student
        FormerStudent ConvertToFormerStudent(long formerClassId, BatchUploadItem item);

        // converts batch upload to batch upload detail
        BatchUploadDetail<FormerStudentDetail> ConvertToBatchUploadDetail(BatchUpload batchUpload);

        // convert to former student form
        FormerStudentForm ConvertToFormerStudentForm(BatchUploadItem item);

        // update batch upload item
        void UpdateBatchUploadItem(FormerStudentForm form, BatchUploadItem item);

    }

}