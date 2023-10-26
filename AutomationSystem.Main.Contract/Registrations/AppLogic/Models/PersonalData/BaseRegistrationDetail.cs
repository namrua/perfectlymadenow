using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Base registration detail
    /// </summary>
    public class BaseRegistrationDetail
    {

        [DisplayName("ID")]
        public long ClassRegistrationId { get; set; }

        [DisplayName("Class ID")]
        public long ClassId { get; set; }

        [DisplayName("Class state")]
        public ClassState ClassState { get; set; }

        [DisplayName("Type of registration code")]
        public RegistrationTypeEnum RegistrationTypeId { get; set; }

        [DisplayName("Type of registration")]
        public string RegistrationType { get; set; }

        [DisplayName("Language code")]
        public LanguageEnum LanguageId { get; set; }

        [DisplayName("Language")]
        [LocalisedText("Metadata", "Language")]
        public string Language { get; set; }

    }

}
