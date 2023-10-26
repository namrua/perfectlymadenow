using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Encapsulates student registration detail
    /// </summary>
    public class RegistrationStudentDetail : BaseRegistrationDetail
    {
       
        [DisplayName("Student's address")]
        [LocalisedText("Metadata", "StudentAddress")]
        public AddressDetail Address { get; set; }

        [DisplayName("Email")]
        [LocalisedText("Metadata", "Email")]
        [EmailAddress]       
        public string Email { get; set; }
        
        [DisplayName("Phone")]
        [LocalisedText("Metadata", "Phone")]
        public string Phone { get; set; }
        public bool CanEdit { get; set; }

        
        public RegistrationStudentDetail()
        {
            Address = new AddressDetail();            
        }

    }
}
