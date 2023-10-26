using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Linq;

namespace AutomationSystem.Main.Core.Profiles.Data.Extensions
{
    public static class ProfileRemoveInactive
    {
        // removes inactive includes for Profile
        public static Profile RemoveInactiveForProfile(Profile entity, ProfileIncludes includes)
        {
            if (entity == null)
                return null;
            if (includes.HasFlag(ProfileIncludes.ClassPreferenceClassPreferenceExpenses))
                entity.ClassPreference.ClassPreferenceExpenses =
                    entity.ClassPreference.ClassPreferenceExpenses.AsQueryable().Active().ToList();
            if (includes.HasFlag(ProfileIncludes.ProfileUsers))
                entity.ProfileUsers = entity.ProfileUsers.AsQueryable().Active().ToList();
            return entity;
        }
    }
}
