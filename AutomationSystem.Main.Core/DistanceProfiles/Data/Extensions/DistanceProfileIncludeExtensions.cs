using System.Data.Entity.Infrastructure;
using AutomationSystem.Main.Core.DistanceProfiles.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.DistanceProfiles.Data.Extensions
{
    public static class DistanceProfileIncludeExtensions
    {
        // adds includes for distance profile
        public static DbQuery<DistanceProfile> AddIncludes (this DbQuery<DistanceProfile> query, DistanceProfileIncludes includes)
        {
            if (includes.HasFlag(DistanceProfileIncludes.Profile))
            {
                query = query.Include("Profile");
            }

            if (includes.HasFlag(DistanceProfileIncludes.PriceList))
            {
                query = query.Include("PriceList");
            }

            if (includes.HasFlag(DistanceProfileIncludes.DistanceCoordinator))
            {
                query = query.Include("DistanceCoordinator");
            }

            if (includes.HasFlag(DistanceProfileIncludes.DistanceCoordinatorAddress))
            {
                query = query.Include("DistanceCoordinator.Address");
            }

            if (includes.HasFlag(DistanceProfileIncludes.ProfileClassPreference))
            {
                query = query.Include("Profile.ClassPreference");
            }

            return query;
        }
    }
}
