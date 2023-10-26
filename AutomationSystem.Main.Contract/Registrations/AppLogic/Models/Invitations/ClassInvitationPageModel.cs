using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations
{
    /// <summary>
    /// Class invitation page model
    /// </summary>
    public class ClassInvitationPageModel
    {
        [DisplayName("Class")]
        public ClassShortDetail Class { get; set; }

        [DisplayName("Registration types")]
        public List<IEnumItem> RegistrationTypes { get; set; }

        [DisplayName("Invitations")]
        public List<ClassInvitationItem> Invitations { get; set; }

        public bool AreInvitationsDisabled { get; set; }
        public string InvitationDisabledMessage { get; set; }

        public bool CanInvite { get; set; }

        public ClassInvitationPageModel()
        {
            RegistrationTypes = new List<IEnumItem>();
            Invitations = new List<ClassInvitationItem>();
        }
    }

}
