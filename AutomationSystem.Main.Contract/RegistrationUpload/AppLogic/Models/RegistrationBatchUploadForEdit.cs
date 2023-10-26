using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;
using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.RegistrationUpload.AppLogic.Models
{
    public class RegistrationBatchUploadForEdit : BatchUploadForEdit<RegistrationBatchUploadForm>
    {
        public List<IEnumItem> RegistrationTypes { get; set; } = new List<IEnumItem>();
    }
}
