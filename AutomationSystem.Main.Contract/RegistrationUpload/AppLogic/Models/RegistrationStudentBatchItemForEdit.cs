using AutomationSystem.Base.Contract.Enums;
using System.Collections.Generic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;

namespace AutomationSystem.Main.Contract.RegistrationUpload.AppLogic.Models
{
    public class RegistrationStudentBatchItemForEdit
    {
        public List<IEnumItem> Countries { get; set; } = new List<IEnumItem>();

        public long BatchUploadItemId { get; set; }

        public long BatchUploadId { get; set; }

        public Dictionary<string, string> OriginalValues { get; set; } = new Dictionary<string, string>();

        public RegistrationStudentForm Form { get; set; } = new RegistrationStudentForm();
    }
}
