using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Encapsulate language localisation summary for all items
    /// </summary>
    public class LanguageLocalisationSummary
    {

        // public properties
        public List<LanguageLocalisationSummaryItem> Items { get; set; }     
        public int FullyLocalisedCount { get; set; }

        // constructor
        public LanguageLocalisationSummary()
        {
            Items = new List<LanguageLocalisationSummaryItem>();
        }

    }
}
