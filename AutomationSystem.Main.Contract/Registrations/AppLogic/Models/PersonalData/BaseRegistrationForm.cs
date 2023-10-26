using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Base registraton form
    /// </summary>
    public class BaseRegistrationForm
    {

        [HiddenInput]
        public long ClassId { get; set; }
        [HiddenInput]
        public long ClassRegistrationId { get; set; }
        [HiddenInput]
        public RegistrationTypeEnum RegistrationTypeId { get; set; }
        [HiddenInput]
        public string InvitationRequest { get; set; }

        [Required]
        [DisplayName("Language")]
        [LocalisedText("Metadata", "Language")]
        [PickInputOptions(NoItemText = "no language", Placeholder = "select language")]
        [LocalisedText("Metadata", "LanguageNoItemText", PickInputOptions.NoItemTextKey)]
        [LocalisedText("Metadata", "LanguagePlaceholder", PickInputOptions.PlaceholderKey)]
        public LanguageEnum? LanguageId { get; set; }

    }

}
