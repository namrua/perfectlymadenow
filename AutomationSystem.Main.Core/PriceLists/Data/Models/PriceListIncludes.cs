using System;

namespace AutomationSystem.Main.Core.PriceLists.Data.Models
{
    /// <summary>
    ///  Determines price list includes
    /// </summary>
    [Flags]
    public enum PriceListIncludes
    {
        None = 0,
        PriceListType = 1 << 0,
        PriceListItems = 1 << 1,
        PriceListItemsRegistrationType = 1 << 2,
        Currency = 1 << 3
    }
}