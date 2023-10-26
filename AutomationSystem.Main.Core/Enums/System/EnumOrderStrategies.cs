using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Main.Core.Enums.System
{

    /// <summary>
    /// Extensions for enum ordering strategies
    /// </summary>
    public static class EnumOrderStrategies
    {

        // sets usa as first country
        public static List<IEnumItem> SortDefaultCountryFirst(this List<IEnumItem> countries)
        {
            var deafultCountryId = (int) LocalisationInfo.DefaultCountry;
            var result = countries.OrderBy(x => x.Id != deafultCountryId).ThenBy(x => x.Description).ToList();
            return result;
        }

    }

}
