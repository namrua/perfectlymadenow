using System.ComponentModel;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview
{

    /// <summary>
    /// Registration last class detail
    /// </summary>
    public class RegistrationLastClassDetail
    {
    
        [DisplayName("Location")]
        [LocalisedText("Metadata", "Location")]
        public string Location { get; set; }
      
        [DisplayName("Month")]       
        [LocalisedText("Metadata", "Month")]      
        public int Month { get; set; }
   
        [DisplayName("Year")]       
        [LocalisedText("Metadata", "Year")]       
        public int Year { get; set; }

    }

}
