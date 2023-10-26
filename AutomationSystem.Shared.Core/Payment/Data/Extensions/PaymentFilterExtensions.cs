using System.Linq;
using AutomationSystem.Shared.Contract.Payment.Data.Model;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Payment.Data.Extensions
{
    /// <summary>
    /// Extensions of filters 
    /// TODO: PLEASE EVERYTIME INCLUDE .Active() query
    /// </summary>
    public static class PaymentFilterExtensions
    {
        // select PayPal entities by filter, includes restriction on Active items
        public static IQueryable<PayPalKey> Filter(this IQueryable<PayPalKey> query, PayPalKeyFilter filter)
        {
            query = query.Active();
            if (filter == null) return query;

            if (filter.UserGroupTypeId.HasValue)
            {
                query = query.Where(x => x.UserGroupTypeId == filter.UserGroupTypeId);
            }

            if (filter.UserGroupId.HasValue)
            {
                query = query.Where(x => x.UserGroupId == filter.UserGroupId);
            }

            if (filter.UserGroupIds != null)
            {
                var userGroupIdsNullable = filter.UserGroupIds.Cast<long?>().ToList();
                query = query.Where(x => userGroupIdsNullable.Contains(x.UserGroupId));
            }

            if (filter.CurrencyId.HasValue)
            {
                query = query.Where(x => x.CurrencyId == filter.CurrencyId);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(e => e.Active == filter.IsActive);
            }

            return query;
        }
    }
}
