using System.ComponentModel;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Addresses.AppLogic.Models
{
    /// <summary>
    /// Addresss filter
    /// </summary>
    [Bind(Include = "FirstName, LastName, Street, CityState, CountryId")]
    public class AddressFilter
    {

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Address lines")]
        public string Street { get; set; }

        [DisplayName("City or state")]
        public string CityState { get; set; }

        [DisplayName("Country")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput)]
        public CountryEnum? CountryId { get; set; }

    }

}
