using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Form for student registration
    /// </summary>
    public class RegistrationStudentForm : BaseRegistrationForm
    {
              
        [DisplayName("Student's address")]
        [LocalisedText("Metadata", "StudentAddress")]
        public AddressForm Address { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Email")]
        [LocalisedText("Metadata", "Email")]
        public string Email { get; set; }

        [MaxLength(15)]
        [DisplayName("Phone")]
        [LocalisedText("Metadata", "Phone")]
        public string Phone { get; set; }

        public RegistrationStudentForm()
        {
            Address = new AddressForm();
        }

    }
}
