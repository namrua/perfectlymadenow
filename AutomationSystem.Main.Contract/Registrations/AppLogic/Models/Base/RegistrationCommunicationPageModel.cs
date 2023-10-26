using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base
{
    /// <summary>
    /// Registration communication page model
    /// </summary>
    public class RegistrationCommunicationPageModel
    {

        public long ClassRegistrationId { get; set; }
        public long ClassId { get; set; }
        public List<EmailListItem> Emails { get; set; }

        // constructor
        public RegistrationCommunicationPageModel()
        {
            Emails = new List<EmailListItem>();
        }

    }

}
