using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// AppLocalisation validation result
    /// </summary>
    public class AppLocalisationValidationResult
    {

        public bool IsValid { get; set; }
        public List<string> ValidationMessages { get; set; }

        // constructor
        public AppLocalisationValidationResult()
        {            
            ValidationMessages = new List<string>();
        }

    }

}
