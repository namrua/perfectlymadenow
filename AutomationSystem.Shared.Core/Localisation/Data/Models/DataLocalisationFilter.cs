using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Core.Localisation.Data.Models
{
    /// <summary>
    /// Filter for queries on DataLocalisationFilter
    /// </summary>
    public class DataLocalisationFilter
    {

        // public properties
        public LanguageEnum? LanguageId { get; set; }
        public string LanguageCode { get; set; }
        public List<long> EntityIds { get; set; }                       // empty = no filtering, gets all entities
        public EntityTypeEnum? EntityType { get; set; }

        // constructor
        public DataLocalisationFilter(LanguageEnum? languageId = null, EntityTypeEnum? entityType = null,
            params long[] entityIds)
        {
            LanguageId = languageId;
            EntityType = entityType;
            EntityIds = new List<long>(entityIds);
        }

    }   
        
}
