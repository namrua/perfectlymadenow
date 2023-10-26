using System.Collections.Generic;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Web.Models
{
    /// <summary>
    /// Encapsulates language selector model
    /// </summary>
    public class LanguageSelectorModel
    {       
     
        public List<PickerItem> SupporteLanguages { get; set; }
        public string CurrentLanguage { get; set; }

        // constructor
        public LanguageSelectorModel()
        {
            SupporteLanguages = new List<PickerItem>();
        }

    }

}