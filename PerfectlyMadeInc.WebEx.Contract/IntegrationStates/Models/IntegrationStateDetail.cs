using System;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models
{
    /// <summary>
    /// WebEx integration status detail
    /// </summary>
    public class IntegrationStateDetail
    {

        [DisplayName("Attendee ID")]
        public long? AtendeeId { get; set; }

        [DisplayName("Last checked")]
        public DateTime? LastChecked { get; set; }

        [DisplayName("State type")]
        public IntegrationStateTypeEnum IntegrationStateTypeId { get; set; }

        [DisplayName("State type code")]
        public string IntegrationStateType { get; set; }

        [DisplayName("Integration error message")]
        public string ErrorMessage { get; set; }


        // address 
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Address line 1")]
        public string Street { get; set; }

        [DisplayName("Address line 2")]
        public string Street2 { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("Zip code")]
        public string ZipCode { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

    }
}
