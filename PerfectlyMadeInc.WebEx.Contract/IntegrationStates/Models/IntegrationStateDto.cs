using System;
using AutomationSystem.Base.Contract.Enums;

namespace PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models
{
    public class IntegrationStateDto
    {   
        public long IntegrationStateId { get; set; }
        public long EventId { get; set; }
        public EntityTypeEnum EntityTypeId { get; set; }
        public long EntityId { get; set; }
        public long? AttendeeId { get; set; }
        public IntegrationStateTypeEnum IntegrationStateTypeId { get; set; }
        public DateTime? LastChecked { get; set; }
        public string ErrorMessage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
    }
}
