using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Reports.Data
{
    public interface IFinancialFormDatabaseLayer
    {
        List<RoyaltyFeeRate> GetRoyaltyFeeRatesByCurrencyId(CurrencyEnum currencyId);
    }
}
