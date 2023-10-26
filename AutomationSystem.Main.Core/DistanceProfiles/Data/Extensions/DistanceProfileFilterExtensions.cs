using System.Linq;
using AutomationSystem.Main.Core.DistanceProfiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.DistanceProfiles.Data.Extensions
{
    public static class DistanceProfileFilterExtensions
    {
        public static IQueryable<DistanceProfile> Filter(this IQueryable<DistanceProfile> query, DistanceProfileFilter filter = null)
        {
            query = query.Active();
            if (filter == null)
            {
                return query;
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive);
            }

            if (filter.ExcludeIds != null)
            {
                query = query.Where(x => !filter.ExcludeIds.Contains(x.DistanceProfileId));
            }

            return query;
        }
    }
}
