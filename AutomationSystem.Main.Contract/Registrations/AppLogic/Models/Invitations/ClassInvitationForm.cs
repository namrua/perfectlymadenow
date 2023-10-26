using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Invitations
{
    /// <summary>
    /// Class invitation form
    /// </summary>
    public class ClassInvitationForm
    {
        [HiddenInput]
        public long ClassRegistrationInvitationId { get; set; }

        [HiddenInput]
        public long? ClassRegistrationId { get; set; }

        [HiddenInput]
        public long ClassId { get; set; }

        [HiddenInput]
        public RegistrationTypeEnum RegistrationTypeId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Language")]
        [PickInputOptions(Placeholder = "select language", NoItemText = "no language")]
        public LanguageEnum? LanguageId { get; set; }

    }

}
