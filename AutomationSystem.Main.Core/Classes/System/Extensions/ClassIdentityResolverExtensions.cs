using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Main.Model;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Main.Core.Classes.System.Extensions
{

    /// <summary>
    /// Identity resolver extensions
    /// </summary>
    public static class ClassIdentityResolverExtensions
    {
        private static readonly Dictionary<ClassCategoryEnum, Entitle> classEntitleMap;
        
        static ClassIdentityResolverExtensions()
        {
            classEntitleMap = new Dictionary<ClassCategoryEnum, Entitle>
            {
                {ClassCategoryEnum.Class, Entitle.MainClasses},
                {ClassCategoryEnum.Lecture, Entitle.MainClasses},
                {ClassCategoryEnum.DistanceClass, Entitle.MainDistanceClasses},
                {ClassCategoryEnum.PrivateMaterialClass, Entitle.MainPrivateMaterialClasses}
            };
        }
        
        public static void CheckEntitleForClass(this IIdentityResolver identityResolver, Class cls)
        {
            var entitle = classEntitleMap[cls.ClassCategoryId];
            identityResolver.CheckEntitleForUserGroup(entitle, UserGroupTypeEnum.MainProfile, cls.ProfileId, EntityTypeEnum.MainClass, cls.ClassId);
        }
        
        public static void CheckEntitleForProfileIdAndClassCategory(this IIdentityResolver identityResolver, long? profileId, ClassCategoryEnum classCategoryId)
        {
            var entitle = classEntitleMap[classCategoryId];
            identityResolver.CheckEntitleForProfileId(entitle, profileId);
        }
        
        public static bool IsEntitleGrantedForClass(this IIdentityResolver identityResolver, Class cls)
        {
            var entitle = classEntitleMap[cls.ClassCategoryId];
            var result = identityResolver.IsEntitleGrantedForUserGroup(entitle, UserGroupTypeEnum.MainProfile, cls.ProfileId);
            return result;
        }

        public static HashSet<ClassCategoryEnum> GetGrantedClassCategories(this IIdentityResolver identityResolver)
        {
            var result = new HashSet<ClassCategoryEnum>();
            foreach (var map in classEntitleMap)
            {
                if (identityResolver.IsEntitleGranted(map.Value))
                {
                    result.Add(map.Key);
                }
            }
            return result;
        }

        public static HashSet<long> GetProfileIdsForClasses(this IIdentityResolver identityResolver)
        {
            var result = new HashSet<long>();
            foreach (var map in classEntitleMap)
            {
                var userGroup = identityResolver.GetGrantedUserGroupsForEntitle(map.Value, UserGroupTypeEnum.MainProfile);
                if (userGroup.GrantedUserGroupIds == null)
                {
                    return null;
                }

                result.UnionWith(userGroup.GrantedUserGroupIds);
            }

            return result;
        }
    }

}
