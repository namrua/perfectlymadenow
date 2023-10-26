using System;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Incidents.AppLogic.Models
{
    /// <summary>
    /// Incident detail
    /// </summary>
    public class IncidentDetail
    {

        [DisplayName("ID")]
        public long IncidentId { get; set; }

        [DisplayName("Parent incident")]
        public long? ParentIncidentId { get; set; }

        [DisplayName("Inner incidents")]
        public int InnerIncidentsCount { get; set; }

        [DisplayName("Related entity type code")]
        public EntityTypeEnum? EntityTypeId { get; set; }

        [DisplayName("Related entity type")]
        public string EntityType { get; set; }

        [DisplayName("Related entity ID")]
        public long? EntityId { get; set; }

        [DisplayName("Incident type code")]
        public IncidentTypeEnum IncidentTypeId { get; set; }

        [DisplayName("Incident type")]
        public string IncidentType { get; set; }

        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Occurrence time")]
        public DateTime Occurred { get; set; }

        [DisplayName("Can be reported")]
        public bool CanBeReported { get; set; }

        [DisplayName("Is reported")]
        public bool IsReported { get; set; }

        [DisplayName("Reporting attempts")]
        public int ReportingAttempts { get; set; }

        [DisplayName("Reporting time")]
        public DateTime? Reported { get; set; }

        [DisplayName("Is resolved")]
        public bool IsResolved { get; set; }

        [DisplayName("Resolving time")]
        public DateTime? Resolved { get; set; }

    }

}
