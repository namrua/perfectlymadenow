using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;
using System.Linq;

namespace AutomationSystem.Main.Core.Profiles.System.Extensions
{
    public static class ProfileIdentityResolverExtensions
    {
        public static ProfileFilter GetGrantedProfilesForEntitle(this IIdentityResolver identityResolver, Entitle entitle)
        {
            var accessLevel = identityResolver.GetGrantedUserGroupsForEntitle(entitle, UserGroupTypeEnum.MainProfile);
            var filter = new ProfileFilter
            {
                ProfileIds = accessLevel.GrantedUserGroupIds?.ToList(),
                IncludeDefaultProfile = accessLevel.IncludeDefaultGroup
            };
            return filter;
        }
        
        public static void CheckEntitleForProfile(this IIdentityResolver identityResolver, Entitle entitle, Profile profile)
        {
            identityResolver.CheckEntitleForUserGroup(entitle, UserGroupTypeEnum.MainProfile, profile.ProfileId,
                EntityTypeEnum.MainProfile, profile.ProfileId);
        }
        
        public static void CheckEntitleForProfileId(this IIdentityResolver identityResolver, Entitle entitle, long? profileId)
        {
            identityResolver.CheckEntitleForUserGroup(entitle, UserGroupTypeEnum.MainProfile, profileId,
                EntityTypeEnum.MainProfile, profileId);
        }

        public static bool? ResolveOnlyWwaEmailTypes(this IIdentityResolver identityResolver, long? profileId)
        {
            if (identityResolver.IsEntitleGrantedForUserGroup(Entitle.MainClasses, UserGroupTypeEnum.MainProfile, profileId))
            {
                return false;
            }

            if (identityResolver.IsEntitleGrantedForUserGroup(Entitle.MainDistanceClasses, UserGroupTypeEnum.MainProfile, profileId))
            {
                return true;
            }

            return null;
        }
    }
}
