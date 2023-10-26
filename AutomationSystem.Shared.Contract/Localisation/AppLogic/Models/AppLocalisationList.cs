using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Localisation.AppLogic.Models
{
    /// <summary>
    /// Application localisation list
    /// </summary>
    public class AppLocalisationList
    {

        // public properties
        public LanguageEnum LanguageId { get; set; }
        public IEnumItem Language { get; set; }
        public List<AppLocalisationListItem> Items { get; set; }

        // constructor
        public AppLocalisationList()
        {
            Items = new List<AppLocalisationListItem>();
        }

    }

}
