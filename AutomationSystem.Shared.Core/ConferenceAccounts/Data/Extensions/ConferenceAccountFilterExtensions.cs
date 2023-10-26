using System.Linq;
using AutomationSystem.Base.Contract.Integration;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.ConferenceAccounts.Data.Extensions
{
    /// <summary>
    /// Extensions of filters 
    /// TODO: PLEASE EVERYTIME INCLUDE .Active() query
    /// </summary>
    public static class ConferenceAccountFilterExtensions
    {
        // selects ConferenceAccount entities by filter, includes restriction on Active items (if includeDeleted is false)
        public static IQueryable<ConferenceAccount> Filter(
            this IQueryable<ConferenceAccount> query,
            ConferenceAccountFilter filter,
            bool includeDeleted = false)
        {
            if (!includeDeleted)
                query = query.Active();
            if (filter == null)
                return query;

            if (filter.ConferenceAccountTypeId.HasValue)
                query = query.Where(x => x.ConferenceAccountTypeId == filter.ConferenceAccountTypeId);
            if (filter.UserGroupTypeId.HasValue)
                query = query.Where(x => x.UserGroupTypeId == filter.UserGroupTypeId);
            if (filter.UserGroupId.HasValue)
                query = query.Where(x => x.UserGroupId == filter.UserGroupId);
            if (filter.UserGroupIds != null)
            {
                var userGroupIdsNullable = filter.UserGroupIds.Cast<long?>().ToList();
                query = query.Where(x => userGroupIdsNullable.Contains(x.UserGroupId));
            }

            return query;
        }
    }
}
