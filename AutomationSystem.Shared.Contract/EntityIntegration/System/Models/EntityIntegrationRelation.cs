using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.EntityIntegration.System.Models
{
    /// <summary>
    /// Determines integration relation definition
    /// </summary>
    public class EntityIntegrationRelation
    {
       
        public IntegrationTypeEnum IntegrationTypeId { get; set; }
        public long? IntegrationEntityId { get; set; }        

    }
}
