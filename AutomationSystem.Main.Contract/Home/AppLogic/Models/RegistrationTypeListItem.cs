using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// registration type list item
    /// </summary>
    public class RegistrationTypeListItem
    {
        [DisplayName("Registration type code")]
        public RegistrationTypeEnum RegistrationTypeId { get; set; }

        [DisplayName("Registration type")]
        [LocalisedText("Metadata", "RegistrationType")]
        public string RegistrationType { get; set; }

        [DisplayName("Price")]
        [LocalisedText("Metadata", "Price")]
        public decimal? Price { get; set; }
    }
}