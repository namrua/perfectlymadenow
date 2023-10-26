using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using System.Data.Entity.Infrastructure;

namespace AutomationSystem.Main.Core.Profiles.Data.Extensions
{
    public static class ProfileIncludeExtensions
    {
        // adds includes for Profile
        public static DbQuery<Profile> AddIncludes(this DbQuery<Profile> query, ProfileIncludes includes)
        {
            if (includes.HasFlag(ProfileIncludes.ClassPreference))
                query = query.Include("ClassPreference");
            if (includes.HasFlag(ProfileIncludes.ClassPreferenceClassPreferenceExpenses))
                query = query.Include("ClassPreference.ClassPreferenceExpenses");
            if (includes.HasFlag(ProfileIncludes.ProfileUsers))
                query = query.Include("ProfileUsers");
            if (includes.HasFlag(ProfileIncludes.ClassPreferenceLocationInfo))
                query = query.Include("ClassPreference.LocationInfo.Address");
            if (includes.HasFlag(ProfileIncludes.ClassPreferenceCurrency))
                query = query.Include("ClassPreference.Currency");
            return query;
        }
    }
}
