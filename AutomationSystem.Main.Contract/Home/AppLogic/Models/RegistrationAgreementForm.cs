using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Registration agreement form
    /// </summary>
    public class RegistrationAgreementForm
    {
        [DisplayName("ID")]
        [HiddenInput]        
        public long ClassRegistrationId { get; set; }

        [Required]
        [DisplayName("Accept agreement")]
        [LocalisedText("Metadata", "AcceptAgreement")]
        public bool AcceptAgreements { get; set; }
    }
}