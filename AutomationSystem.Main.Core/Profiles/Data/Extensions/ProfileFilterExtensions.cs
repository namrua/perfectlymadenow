using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Linq;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;

namespace AutomationSystem.Main.Core.Profiles.Data.Extensions
{
    public static class ProfileFilterExtensions
    {
        // selects Profile etities by filter, includes restriction on Active items
        public static IQueryable<Profile> Filter(this IQueryable<Profile> query, ProfileFilter filter)
        {
            query = query.Active();
            if (filter == null)
            {
                return query;
            }

            if (filter.ProfileIds != null)
            {
                query = query.Where(x => filter.ProfileIds.Contains(x.ProfileId));
            }

            if (filter.ExcludeProfileIds != null)
            {
                query = query.Where(x => !filter.ExcludeProfileIds.Contains(x.ProfileId));
            }

            return query;
        }
    }
}
