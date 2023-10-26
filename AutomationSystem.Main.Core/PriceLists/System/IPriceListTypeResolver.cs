using AutomationSystem.Base.Contract.Enums;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.PriceLists.System
{
    public interface IPriceListTypeResolver
    {
        HashSet<RegistrationTypeEnum> GetRegistrationTypesForPriceList(PriceListTypeEnum priceListTypeId);

        bool IsPriceListAllowedForClassCategoryId(PriceListTypeEnum priceListTypeId, ClassCategoryEnum classCategoryId);

        PriceListTypeEnum GetPriceListTypeIdAllowedForClassCategoryId(ClassCategoryEnum classCategoryId);
    }
}
