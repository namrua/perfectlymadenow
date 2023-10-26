using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations
{
    /// <summary>
    /// Encapsulates information of conversation invitation
    /// </summary>
    public class ClassInvitationItem
    {
        
        public long ClassRegistrationInvitationId { get; set; }
        public long? ClassRegistrationId { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("State")]
        public ClassInvitationState State { get; set; }

    }

}
