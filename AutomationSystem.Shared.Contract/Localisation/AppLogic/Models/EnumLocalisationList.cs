using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Enum localisation list
    /// </summary>
    public class EnumLocalisationList
    {

        // public properties
        public LanguageEnum LanguageId { get; set; }
        public IEnumItem Language { get; set; }
        public EnumType EnumType { get; set; }
        public List<EnumLocalisationListItem> Items { get; set; }

        // constructor
        public EnumLocalisationList()
        {
            Items = new List<EnumLocalisationListItem>();
        }

    }

}
