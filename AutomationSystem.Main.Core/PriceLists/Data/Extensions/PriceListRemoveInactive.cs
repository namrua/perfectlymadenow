using System.Linq;
using AutomationSystem.Main.Core.PriceLists.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.PriceLists.Data.Extensions
{
    public static class PriceListRemoveInactive
    {
        public static PriceList RemoveInactiveForPriceList(PriceList entity, PriceListIncludes includes)
        {
            if (entity == null)
            {
                return null;
            }

            if (includes.HasFlag(PriceListIncludes.PriceListItems)
                || includes.HasFlag(PriceListIncludes.PriceListItemsRegistrationType))
            {
                entity.PriceListItems = entity.PriceListItems.AsQueryable().Active().ToList();
            }

            return entity;
        }
    }
}
