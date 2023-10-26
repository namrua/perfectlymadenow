using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments
{
    /// <summary>
    /// Payload address detail
    /// </summary>
    public class PayloadAddressDetail
    {

        [DisplayName("Recipient name")]
        public string RecipientName { get; set; }

        [DisplayName("Address line 1")]
        public string Line1 { get; set; }

        [DisplayName("Address line 2")]
        public string Line2 { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Postal code")]
        public string PostalCode { get; set; }

        [DisplayName("Country code")]
        public string CountryCode { get; set; }


        [DisplayName("Street")] public string FullStreet { get; set; }
        [DisplayName("City/State")] public string FullCity { get; set; }
    }
}
