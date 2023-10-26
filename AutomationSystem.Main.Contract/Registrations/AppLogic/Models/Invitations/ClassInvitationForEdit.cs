using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations
{

    /// <summary>
    /// Class invitation for edit
    /// </summary>
    public class ClassInvitationForEdit
    {
        public ClassInvitationForm Form { get; set; }
        public List<IEnumItem> Languages { get; set; }
        
        public ClassInvitationForEdit()
        {
            Form = new ClassInvitationForm();
            Languages = new List<IEnumItem>();
        }
    }

}
