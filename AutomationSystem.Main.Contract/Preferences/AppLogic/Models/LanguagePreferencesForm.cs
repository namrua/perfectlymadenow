using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Preferences.AppLogic.Models
{
    /// <summary>
    /// Language form
    /// </summary>
    public class LanguagePreferencesForm
    {
        // public properties
        [Required]
        [DisplayName("Is supported")]
        public string[] SupportedLanguages { get; set; }
        
        public LanguagePreferencesForm()
        {
            SupportedLanguages = new string[0];
        }
    }

}
