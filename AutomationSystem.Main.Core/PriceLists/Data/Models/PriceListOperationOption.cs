using System;

namespace AutomationSystem.Main.Core.PriceLists.Data.Models
{
    /// <summary>
    /// Determines special operation options on price list entities
    /// </summary>
    [Flags]
    public enum PriceListOperationOption
    {
        None = 0,
        KeepOwnerId = 1 << 0,                       // does not change OwnerId
        KeepStatus = 1 << 1,                        // does not change approved and discard status
        CheckPriceListType = 1 << 2                 // checks if price list type is consistent
    }
}