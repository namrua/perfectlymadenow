using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Application localisation for edit
    /// </summary>
    public class AppLocalisationForEdit
    {

        // public properties         
        public IEnumItem Language { get; set; }
        public string Module { get; set; }
        public string Label { get; set; }

        [DisplayName("Default html text")]
        public string OriginalValue { get; set; }

        public bool IsDefaultLanguage { get; set; }
        public AppLocalisationValidationResult ValidationResult { get; set; }
       
        public AppLocalisationForm Form { get; set; }


        // constructor
        public AppLocalisationForEdit()
        {
            Form = new AppLocalisationForm();
            ValidationResult = new AppLocalisationValidationResult();
        }

    }

}
