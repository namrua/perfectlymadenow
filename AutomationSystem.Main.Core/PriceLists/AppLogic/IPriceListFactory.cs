using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.PriceLists.AppLogic.Models;

namespace AutomationSystem.Main.Core.PriceLists.AppLogic
{
    public interface IPriceListFactory
    {
        PriceListForEdit InitializePriceListForEdit(PriceListTypeEnum priceListTypeId);
    }
}
