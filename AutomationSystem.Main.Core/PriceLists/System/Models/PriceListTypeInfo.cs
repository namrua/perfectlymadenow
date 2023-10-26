using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.PriceLists.System.Models
{
    /// <summary>
    /// Encapsulates information about price list
    /// </summary>
    public class PriceListTypeInfo
    {
        public HashSet<RegistrationTypeEnum> RegistrationTypesInPriceList { get; set; }
        public HashSet<ClassCategoryEnum> AllowedClassCategoryIds { get; set; }

        public PriceListTypeInfo(
            IEnumerable<RegistrationTypeEnum> registrationTypesInPriceList = null,
            HashSet<ClassCategoryEnum> allowedClassCategoryIds = null)
        {
            RegistrationTypesInPriceList = registrationTypesInPriceList == null
                ? new HashSet<RegistrationTypeEnum>()
                : new HashSet<RegistrationTypeEnum>(registrationTypesInPriceList);
            AllowedClassCategoryIds = allowedClassCategoryIds ?? new HashSet<ClassCategoryEnum>();
        }
    }
}
