using System.Collections.Generic;
using AutomationSystem.Main.Contract.Files.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Documents
{
    /// <summary>
    /// Registration documents page model
    /// </summary>
    public class RegistrationDocumentsPageModel
    {

        public long ClassId { get; set; }
        public long ClassRegistrationId { get; set; }
        public bool AreCertificatesAllowed { get; set; }
        public List<EntityFileDetail> Files { get; set; }

        // constructor
        public RegistrationDocumentsPageModel()
        {
            Files = new List<EntityFileDetail>();
        }

    }
}
