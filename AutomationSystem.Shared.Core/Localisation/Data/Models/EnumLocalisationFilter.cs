using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Core.Localisation.Data.Models
{
    /// <summary>
    /// Filter for queries on EnumLocalisation
    /// </summary>
    public class EnumLocalisationFilter
    {

        // public properties
        public string LanguageCode { get; set; }        
        public LanguageEnum? LanguageId { get; set; }       
        public EnumTypeEnum? EnumTypeId { get; set; }
        public int? ItemId { get; set; }
        public IEnumerable<int> ItemIds { get; set; }

        // constructor
        public EnumLocalisationFilter(LanguageEnum? languageId = null, EnumTypeEnum? enumTypeId = null, int? itemId = null)
        {
            LanguageId = languageId;
            EnumTypeId = enumTypeId;
            ItemId = itemId;         
        }

    }
        
}
