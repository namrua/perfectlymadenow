using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Addresses.AppLogic.Models
{
    /// <summary>
    /// Form address 
    /// </summary>
    public class IncompleteAddressForm
    {

        [Required]
        [MaxLength(64)]
        [DisplayName("First name")]
        [LocalisedText("Metadata", "FirstName")]
        public string FirstName { get; set; }
      
        [MaxLength(64)]
        [DisplayName("Last name")]
        [LocalisedText("Metadata", "LastName")]
        public string LastName { get; set; }
        
        [MaxLength(64)]
        [DisplayName("Address line 1")]
        [LocalisedText("Metadata", "Street")]
        public string Street { get; set; }

        [MaxLength(64)]
        [DisplayName("Address line 2")]
        [LocalisedText("Metadata", "Street2")]
        public string Street2 { get; set; }
       
        [MaxLength(64)]
        [DisplayName("City")]
        [LocalisedText("Metadata", "City")]
        public string City { get; set; }

        [MaxLength(64)]
        [DisplayName("State")]
        [LocalisedText("Metadata", "State")]
        public string State { get; set; }

        [Required]
        [DisplayName("Country")]
        [LocalisedText("Metadata", "Country")]
        [PickInputOptions(Placeholder = "select country", NoItemText = "no country")]
        [LocalisedText("Metadata", "CountryPlaceholder", PickInputOptions.PlaceholderKey)]
        [LocalisedText("Metadata", "CountryNoItemText", PickInputOptions.NoItemTextKey)]
        public CountryEnum? CountryId { get; set; }
     
        [MaxLength(16)]
        [DisplayName("Zip code")]
        [LocalisedText("Metadata", "ZipCode")]
        public string ZipCode { get; set; }

    }

}
