using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;

namespace AutomationSystem.Main.Contract.Addresses.AppLogic.Models
{
    /// <summary>
    /// Address detail
    /// </summary>
    public class AddressDetail
    {

        [DisplayName("ID")]
        public long AddressId { get; set; }

        [DisplayName("First name")]
        [LocalisedText("Metadata", "FirstName")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [LocalisedText("Metadata", "LastName")]
        public string LastName { get; set; }

        [DisplayName("Address line 1")]
        [LocalisedText("Metadata", "Street")]
        public string Street { get; set; }

        [DisplayName("Address line 2")]
        [LocalisedText("Metadata", "Street2")]
        public string Street2 { get; set; }

        [DisplayName("City")]
        [LocalisedText("Metadata", "City")]
        public string City { get; set; }

        [DisplayName("State")]
        [LocalisedText("Metadata", "State")]
        public string State { get; set; }

        [DisplayName("Country code")]
        public CountryEnum CountryId { get; set; }

        [DisplayName("Country")]
        [LocalisedText("Metadata", "Country")]
        public string Country { get; set; }

        [DisplayName("Zip code")]
        [LocalisedText("Metadata", "ZipCode")]
        public string ZipCode { get; set; }


        // helpers
        [DisplayName("Name")]
        [LocalisedText("Metadata", "Name")]
        public string FullName { get; set; }

        [DisplayName("Street")] public string FullStreet { get; set; }
        [DisplayName("City/State")] public string FullCity { get; set; }

    }

}
