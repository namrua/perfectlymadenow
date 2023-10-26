using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.Reports.Data
{
    public class FinancialFormDatabaseLayer : IFinancialFormDatabaseLayer
    {
        public List<RoyaltyFeeRate> GetRoyaltyFeeRatesByCurrencyId(CurrencyEnum currencyId)
        {
            using (var context = new MainEntities())
            {
                var result = context.RoyaltyFeeRates.Active().Where(x => x.CurrencyId == currencyId).ToList();
                return result;
            }
        }
    }
}
