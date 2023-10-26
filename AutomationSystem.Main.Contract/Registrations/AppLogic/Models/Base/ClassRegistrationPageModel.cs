using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base
{
    /// <summary>
    /// Encapsulates data for registration page model
    /// </summary>
    public class ClassRegistrationPageModel
    {
        [DisplayName("Class")]
        public ClassShortDetail Class { get; set; }

        [DisplayName("Registration types")]
        public List<IEnumItem> RegistrationTypes { get; set; }

        [DisplayName("Registrations for approval")]
        public List<RegistrationListItem> RegistrationsForApprove { get; set; }
        public bool CanAddNewRegistration { get; set; }

        public bool CanBatchUpload { get; set; }

        public List<BatchUploadListItem> RegistrationBatchUploads { get; set; } = new List<BatchUploadListItem>();

        // constructor  
        public ClassRegistrationPageModel()
        {
            Class = new ClassShortDetail();
            RegistrationTypes = new List<IEnumItem>();
            RegistrationsForApprove = new List<RegistrationListItem>();
        }
    }
}