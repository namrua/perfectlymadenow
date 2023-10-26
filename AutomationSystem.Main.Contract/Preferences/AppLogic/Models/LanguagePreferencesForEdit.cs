using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Preferences.AppLogic.Models
{
    /// <summary>
    /// language extended model
    /// </summary>
    public class LanguagePreferencesForEdit
    {
        // public properties
        [DisplayName]
        public List<IEnumItem> AllLanguages { get; set; }
        public string DefaultLanguage { get; set; }
        public LanguagePreferencesForm Form { get; set; }

        // constructor
        public LanguagePreferencesForEdit()
        {
            AllLanguages = new List<IEnumItem>();
            Form = new LanguagePreferencesForm();
        }
    }

}
