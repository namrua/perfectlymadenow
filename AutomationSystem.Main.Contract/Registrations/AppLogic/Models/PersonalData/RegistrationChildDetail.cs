using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Encapsulates child registration detail
    /// </summary>
    public class RegistrationChildDetail : BaseRegistrationDetail
    {                

        [DisplayName("Parent's/Guardian's address")]
        [LocalisedText("Metadata", "ParentAddress")]
        public AddressDetail ParentAddress { get; set; }

        [DisplayName("Email")]
        [LocalisedText("Metadata", "Email")]
        [EmailAddress]        
        public string ParentEmail { get; set; }

        [DisplayName("Phone")]
        [LocalisedText("Metadata", "Phone")]
        public string ParentPhone { get; set; }


        [DisplayName("Child's address")]
        [LocalisedText("Metadata", "ChildAddress")]
        public AddressDetail ChildAddress { get; set; }

        [DisplayName("Email")]
        [LocalisedText("Metadata", "Email")]
        [EmailAddress]       
        public string ChildEmail { get; set; }
        public bool CanEdit { get; set; }
        
        
        public RegistrationChildDetail()
        {
            ParentAddress = new AddressDetail();
            ChildAddress = new AddressDetail();
        }

    }
}
