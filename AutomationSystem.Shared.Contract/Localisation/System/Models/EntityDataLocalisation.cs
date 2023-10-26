using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Localisation.System.Models
{

    /// <summary>
    /// Returns composed localisation data types
    /// </summary>
    public class EntityDataLocalisation
    {

        // gets composed localisation data types
        public LanguageEnum LanguageId { get; set; }
        public EntityTypeEnum EntityTypeId { get; set; }
        public long EntityId { get; set; }
        public Dictionary<string, string> ColumnMap { get; set; }

        // constructor
        public EntityDataLocalisation()
        {
            ColumnMap = new Dictionary<string, string>();
        }

        // constructor
        public EntityDataLocalisation(EntityTypeEnum entityType, long entityId, LanguageEnum languageId)
        {
            LanguageId = languageId;
            EntityTypeId = entityType;
            EntityId = entityId;
            ColumnMap = new Dictionary<string, string>();
        }

        // adds column value
        public void AddColumnValue(string columnName, string value)
        {
            ColumnMap[columnName] = value;            
        }

        // get string by column, or null when it is empty or does not exists
        public string GetColumnValue(string columnName)
        {
            if (!ColumnMap.TryGetValue(columnName, out var result))
                return null;
            return string.IsNullOrEmpty(result) ? null : result;
        }

    }

}
