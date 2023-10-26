using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudentsBatchUploads;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.FormerClasses.AppLogic
{
    public interface IFormerStudentBatchUploadFactory
    {
        FormerStudentBatchItemForEdit CreateFormerStudentBatchItemForEdit(BatchUploadItem batchUploadItem);
    }
}
