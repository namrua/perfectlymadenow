using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Encapsulates WWA registration detail
    /// </summary>
    public class RegistrationWwaDetail : BaseRegistrationDetail
    {          

        [DisplayName("Participant's address")]
        [LocalisedText("Metadata", "ParticipantAddress")]
        public AddressDetail ParticipantAddress { get; set; }
       
        [DisplayName("Registrant's address")]
        [LocalisedText("Metadata", "RegistrationAddress")]
        public AddressDetail RegistrantAddress { get; set; }

        [DisplayName("Email")]
        [LocalisedText("Metadata", "Email")]
        [EmailAddress]
        public string RegistrantEmail { get; set; }
        public bool CanEdit { get; set; }

        
        public RegistrationWwaDetail()
        {
            ParticipantAddress = new AddressDetail();
            RegistrantAddress = new AddressDetail();
        }

    }
}
