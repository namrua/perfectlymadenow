using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Enum type list
    /// </summary>
    public class EnumTypeList
    {

        // public properties
        public LanguageEnum LanguageId { get; set; }
        public IEnumItem Language { get; set; }
        public List<EnumTypeListItem> Items { get; set; }        

        // constructor
        public EnumTypeList()
        {
            Items = new List<EnumTypeListItem>();
        }

    }
}
