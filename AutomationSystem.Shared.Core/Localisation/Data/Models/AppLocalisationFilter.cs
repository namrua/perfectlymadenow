using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Core.Localisation.Data.Models
{
    /// <summary>
    /// Filter for queries on AppLocalisation
    /// </summary>
    public class AppLocalisationFilter
    {

        // public properties
        public LanguageEnum? LanguageId { get; set; }
        public HashSet<string> SupportedLanguages { get; set; }
        public string Module { get; set; }
        public string Label { get; set; }

        // constructor
        public AppLocalisationFilter(LanguageEnum? languageId = null, string module = null, string label = null, HashSet<string> supportedLanguages = null)
        {
            LanguageId = languageId;
            Module = module;
            Label = label;
            SupportedLanguages = supportedLanguages;
        }

    } 
}
