using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Core.Identities.System.Model;

namespace AutomationSystem.Shared.Core.Identities.System
{
    /// <summary>
    /// Encapsulates mapping of entitles - roles - user groups
    /// </summary>
    public static class EntitleMapper
    {

        private static readonly Dictionary<UserRoleTypeEnum, HashSet<Entitle>> entitlesByRole;                              // entitles partitioned by roles (optimized for entitles set operations)
        private static readonly Dictionary<Entitle, List<EntitlePermission>> permissionsByEntitle;                          // permissions partitioned by entitles

        // constructor
        static EntitleMapper()
        {
            // permission table - source
            var permissionTable = GetEntitlePermissionDefinition();

            // entitles partitioned by roles (optimized for entitles set operations)
            entitlesByRole = permissionTable
                .GroupBy(x => x.RoleId)
                .ToDictionary(x => x.Key, y => new HashSet<Entitle>(y.Select(z => z.Entitle)));

            // permissions partitioned by entitles
            permissionsByEntitle = permissionTable
                .GroupBy(x => x.Entitle)
                .ToDictionary(x => x.Key, y => y.ToList());
        }


        #region entitle permission definition

        /// <summary>
        /// Encapsulates entitle permissions
        /// </summary>
        public class EntitlePermission
        {

            public UserRoleTypeEnum RoleId { get; }
            public Entitle Entitle { get; }
            public UserGroupTypeEnum? UserGroupTypeId { get; }
            public UserGroupAccessLevel UserGroupAccessLevel { get; }
            public bool IncludeDefaultUserGroup { get; }


            // constructor
            public EntitlePermission(UserRoleTypeEnum roleId, Entitle entitle, UserGroupTypeEnum? ugTypeId = null,
                UserGroupAccessLevel ugAccessLevel = UserGroupAccessLevel.NoAccess, bool includeDefaultUg = false)
            {
                RoleId = roleId;
                Entitle = entitle;
                UserGroupTypeId = ugTypeId;
                UserGroupAccessLevel = ugAccessLevel;
                IncludeDefaultUserGroup = includeDefaultUg;
            }

        }


        /// <summary>
        /// Gets entitle permission definition
        /// </summary>
        /// <returns></returns>
        private static List<EntitlePermission> GetEntitlePermissionDefinition()
        {
            var result = new List<EntitlePermission>
            {
                // admin
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.CoreLocalisation),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.CoreEmailTemplates),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.CoreEmailTemplatesIntegration),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.CoreUserAccounts),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.CoreIncidents),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.CorePreferences),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.CorePayPalKeys, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.CoreJobs),

                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainProfiles, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainPersons, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All, true),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainPersonsReadOnly, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All, true),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainClasses, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainDistanceClasses, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainMaterials, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainPrivateMaterialClasses, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainFormerClasses),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainFormerClassesReadOnly),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainPriceLists),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainPreferences),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainDistanceProfile),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainDistanceClassTemplate),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.MainContacts, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),

                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.WebExAccounts, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.Administrator, Entitle.WebExPrograms, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),

                // super coordinator
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.CoreEmailTemplatesIntegration),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.CoreUserAccountsRestricted),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.CorePayPalKeys, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),

                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainProfiles, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainPersons, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All, true),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainPersonsReadOnly, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All, true),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainClasses, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainDistanceClasses, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainMaterials, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainPrivateMaterialClasses, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainFormerClasses),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainFormerClassesReadOnly),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainPriceLists),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainPreferences),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainDistanceProfile),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainDistanceClassTemplate),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.MainContacts, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),
                
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.WebExAccounts, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),
                new EntitlePermission(UserRoleTypeEnum.SuperCoordinator, Entitle.WebExPrograms, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.All),

                // coordinator
                new EntitlePermission(UserRoleTypeEnum.Coordinator, Entitle.CoreEmailTemplatesIntegration),
                new EntitlePermission(UserRoleTypeEnum.Coordinator, Entitle.CorePayPalKeys, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),

                new EntitlePermission(UserRoleTypeEnum.Coordinator, Entitle.MainProfiles, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),
                new EntitlePermission(UserRoleTypeEnum.Coordinator, Entitle.MainPersons, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),
                new EntitlePermission(UserRoleTypeEnum.Coordinator, Entitle.MainPersonsReadOnly, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned, includeDefaultUg: true),
                new EntitlePermission(UserRoleTypeEnum.Coordinator, Entitle.MainClasses, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),
                new EntitlePermission(UserRoleTypeEnum.Coordinator, Entitle.MainMaterials, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),

                new EntitlePermission(UserRoleTypeEnum.Coordinator, Entitle.WebExAccounts, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),
                new EntitlePermission(UserRoleTypeEnum.Coordinator, Entitle.WebExPrograms, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),

                // distance coordinator
                new EntitlePermission(UserRoleTypeEnum.DistanceCoordinator, Entitle.CoreEmailTemplatesIntegration),
                new EntitlePermission(UserRoleTypeEnum.DistanceCoordinator, Entitle.CorePayPalKeys, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),

                new EntitlePermission(UserRoleTypeEnum.DistanceCoordinator, Entitle.MainProfiles, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),
                new EntitlePermission(UserRoleTypeEnum.DistanceCoordinator, Entitle.MainPersons, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),
                new EntitlePermission(UserRoleTypeEnum.DistanceCoordinator, Entitle.MainPersonsReadOnly, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned, includeDefaultUg: true),
                new EntitlePermission(UserRoleTypeEnum.DistanceCoordinator, Entitle.MainDistanceClasses, UserGroupTypeEnum.MainProfile, UserGroupAccessLevel.OnlyAssigned),

                // translator
                new EntitlePermission(UserRoleTypeEnum.Translator, Entitle.CoreLocalisation),
                new EntitlePermission(UserRoleTypeEnum.Translator, Entitle.CoreEmailTemplates),

            };
            return result;
        }

        #endregion


        // determines whether the entitle is granted for roles
        public static bool IsEntitleGrantedForAnyRole(Entitle entitle, HashSet<UserRoleTypeEnum> roles)
        {
            var permissions = GetEntitlePermissions(entitle);
            var result = permissions.Any(x => roles.Contains(x.RoleId));
            return result;
        }


        // determines whether any of entitles are granted for any role
        public static bool IsAnyEntitlesGrantedForAnyRole(HashSet<Entitle> entitles, IEnumerable<UserRoleTypeEnum> roles)
        {
            foreach (var role in roles)
            {
                if (!entitlesByRole.TryGetValue(role, out var roleEntitles))
                    continue;
                if (roleEntitles.Any(entitles.Contains))
                    return true;
            }
            return false;
        }


        // determines higher access level for given entitle, user group and roles
        public static UserGroupAccessLevelWithDefaults GetAccessLevelByEntitleUserGroupTypeAndRoles(Entitle entitle,
            UserGroupTypeEnum? userGroupTypeId, HashSet<UserRoleTypeEnum> roles)
        {
            var entitlePermissions = GetEntitlePermissions(entitle);
            var ugAndRolesPermissions = entitlePermissions.Where(x => roles.Contains(x.RoleId) && x.UserGroupTypeId == userGroupTypeId).ToArray();
            var result = ComputeUserGroupAccessLevelWithDefaultsByPermissions(ugAndRolesPermissions);
            return result;
        }


        #region private methods

        // gets list of permissions by entitle
        private static List<EntitlePermission> GetEntitlePermissions(Entitle entitle)
        {
            return permissionsByEntitle.TryGetValue(entitle, out var result) ? result : new List<EntitlePermission>();
        }

        // computes UserGroupAccessLevelWithDefaults from permissions
        private static UserGroupAccessLevelWithDefaults ComputeUserGroupAccessLevelWithDefaultsByPermissions(EntitlePermission[] permissions)
        {
            var result = new UserGroupAccessLevelWithDefaults(
                permissions.Length == 0 ? UserGroupAccessLevel.NoAccess : permissions.Max(x => x.UserGroupAccessLevel), 
                permissions.Any(x => x.IncludeDefaultUserGroup));
            return result;
        }

        #endregion

    }

}
