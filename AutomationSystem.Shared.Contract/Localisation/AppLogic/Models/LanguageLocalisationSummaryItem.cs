using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Encapsulation language localisation item summary
    /// </summary>
    public class LanguageLocalisationSummaryItem
    {

        // public properties
        public LanguageEnum LanguageId { get; set; }

        [DisplayName("Code")]
        public string LanguageCode { get; set; }

        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("Localised items")]
        public int LocalisedCount { get; set; }
        public bool IsComplete { get; set; }

    }

}
