using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Form for child registration
    /// </summary>
    public class RegistrationChildForm : BaseRegistrationForm
    {      

        [DisplayName("Parent's/Guardian's address")]
        [LocalisedText("Metadata", "ParentAddress")]
        public AddressForm ParentAddress { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Email")]
        [LocalisedText("Metadata", "Email")]
        public string ParentEmail { get; set; }

        [MaxLength(15)]
        [DisplayName("Phone")]
        [LocalisedText("Metadata", "Phone")]
        public string ParentPhone { get; set; }


        [DisplayName("Child's address")]
        [LocalisedText("Metadata", "ChildAddress")]
        public AddressForm ChildAddress { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Email")]
        [LocalisedText("Metadata", "Email")]
        public string ChildEmail { get; set; }

       
        public RegistrationChildForm()
        {
            ParentAddress = new AddressForm();
            ChildAddress = new AddressForm();
        }

    }
}
