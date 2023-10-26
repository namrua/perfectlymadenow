using System.Data.Entity.Infrastructure;
using AutomationSystem.Main.Core.PriceLists.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.PriceLists.Data.Extensions
{
    public static class PriceListIncludeExtensions
    {
        public static DbQuery<PriceList> AddIncludes(this DbQuery<PriceList> query, PriceListIncludes includes)
        {
            if (includes.HasFlag(PriceListIncludes.PriceListType))
                query = query.Include("PriceListType");
            if (includes.HasFlag(PriceListIncludes.PriceListItems))
                query = query.Include("PriceListItems");
            if (includes.HasFlag(PriceListIncludes.PriceListItemsRegistrationType))
                query = query.Include("PriceListItems.RegistrationType");
            if (includes.HasFlag(PriceListIncludes.Currency))
                query = query.Include("Currency");
            return query;
        }
    }
}
