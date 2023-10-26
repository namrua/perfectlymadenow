using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Form for WWA registration
    /// </summary>
    public class RegistrationWwaForm : BaseRegistrationForm
    {

        [DisplayName("Participant's address")]
        [LocalisedText("Metadata", "ParticipantAddress")]
        public IncompleteAddressForm ParticipantAddress { get; set; }

        [DisplayName("Registrant's address")]
        [LocalisedText("Metadata", "RegistrationAddress")]
        public AddressForm RegistrantAddress { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Email")]
        [LocalisedText("Metadata", "Email")]
        public string RegistrantEmail { get; set; }


        public RegistrationWwaForm()
        {
            ParticipantAddress = new IncompleteAddressForm();
            RegistrantAddress = new AddressForm();
        }
    }
}
