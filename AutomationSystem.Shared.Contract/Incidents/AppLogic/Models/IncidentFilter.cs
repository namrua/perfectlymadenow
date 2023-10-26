using System.ComponentModel;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Incidents.AppLogic.Models
{
    /// <summary>
    /// Incident filter
    /// </summary>
    [Bind(Include= "ExcludeReported, IncludeResolved, EntityType, EntityId, ParentIncidentId")]
    public class IncidentFilter
    {

        [DisplayName("Exclude reported")]
        public bool ExcludeReported { get; set; }

        [DisplayName("Include resolved")]
        public bool IncludeResolved { get; set; }
        
        [DisplayName("Entity type")]
        public EntityTypeEnum? EntityType { get; set; }

        [DisplayName("Entity ID")]
        public long? EntityId { get; set; }

        [DisplayName("Parent incident")]
        public long? ParentIncidentId { get; set; }
       
    }
}
