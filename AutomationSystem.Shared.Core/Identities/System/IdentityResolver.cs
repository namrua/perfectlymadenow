using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Contract.Identities.AppLogic.Extensions;

namespace AutomationSystem.Shared.Core.Identities.System
{
    /// <summary>
    /// Grants entitles and user groups by user identity
    /// </summary>
    public class IdentityResolver : IIdentityResolver
    {

        private readonly IIdentityProvider provider;

        // constructor
        public IdentityResolver(IIdentityProvider provider)
        {
            this.provider = provider;
        }

        // returns current IIdentity
        public IIdentity GetCurrentIdentity()
        {
            return provider.GetCurrentIdentity();
        }

        // resolves owner ID
        public int GetOwnerId()
        {
            var result = provider.GetCurrentIdentity().GetId();
            return result;
        }


        // resolves whether entitle is granted
        public bool IsEntitleGranted(Entitle entitle)
        {
            var roles = provider.GetCurrentIdentity().GetRoles();
            var result = EntitleMapper.IsEntitleGrantedForAnyRole(entitle, roles);
            return result;
        }


        // checks whether Entitle is granted, if no - throws EntitleAccessDeniedException
        public void CheckEntitle(Entitle entitle)
        {
            if (IsEntitleGranted(entitle)) return;
            throw EntitleAccessDeniedException.New(entitle, provider.GetCurrentIdentity());
        }


        // resolves whether any entitle is granted
        public bool IsAnyEntitleGranted(HashSet<Entitle> entitles)
        {
            var roles = provider.GetCurrentIdentity().GetRoles();
            var result = EntitleMapper.IsAnyEntitlesGrantedForAnyRole(entitles, roles);
            return result;
        }


        // resolves whether entitle for entity in usergroup is granted
        public bool IsEntitleGrantedForUserGroup(Entitle entitle, UserGroupTypeEnum? userGroupTypeId, long? userGroupId)
        {
            // gets all permissions by roles and user group type - determines higher access level
            var identity = provider.GetCurrentIdentity();
            var roles = identity.GetRoles();
            var accessLevel = EntitleMapper.GetAccessLevelByEntitleUserGroupTypeAndRoles(entitle, userGroupTypeId, roles);

            // tests for default user group (null) or for non-default user group
            var result = !userGroupId.HasValue 
                ? accessLevel.IncludeDefaultUserGroup                                                                                          
                : IsAccessLevelGrantedForNonDefaultUserGroup(userGroupTypeId, userGroupId, accessLevel.UserGroupAccessLevel, identity);     
            return result;
        }

        // checks whether Entitle is granted for user group, if no - throws EntitleAccessDeniedException
        public void CheckEntitleForUserGroup(Entitle entitle, UserGroupTypeEnum? userGroupTypeId, long? userGroupId,
            EntityTypeEnum? entityTypeId = null, long? entityId = null, string entityType = null)
        {
            if (IsEntitleGrantedForUserGroup(entitle, userGroupTypeId, userGroupId)) return;
            throw EntitleAccessDeniedException.New(entitle, provider.GetCurrentIdentity()).AddId(userGroupTypeId, userGroupId, entityTypeId, entityId, entityType);
        }


        // gets granted user groups for given entitle and user group type
        public UserGroupsForEntitle GetGrantedUserGroupsForEntitle(Entitle entitle, UserGroupTypeEnum userGroupTypeId)
        {
            // gets all permissions by roles and user group type - determines higher access level
            var identity = provider.GetCurrentIdentity();
            var roles = identity.GetRoles();
            var accessLevel = EntitleMapper.GetAccessLevelByEntitleUserGroupTypeAndRoles(entitle, userGroupTypeId, roles);

            // converts to UserGroupForEntitle with useing group membership
            var result = new UserGroupsForEntitle(entitle, userGroupTypeId)
            {
                UserGroupAccessLevel = accessLevel.UserGroupAccessLevel,
                IncludeDefaultGroup = accessLevel.IncludeDefaultUserGroup,
                GrantedUserGroupIds = GetGrantedUserGroupIdsByAccessLevel(accessLevel.UserGroupAccessLevel, userGroupTypeId, identity)
            };
            return result;
        }


        #region private methods

        // resolves whether access level is granted for 
        private static bool IsAccessLevelGrantedForNonDefaultUserGroup(UserGroupTypeEnum? userGroupTypeId, long? userGroupId,
            UserGroupAccessLevel accessLevel, IIdentity identity)
        {
            switch (accessLevel)
            {
                // no access - denies access
                case UserGroupAccessLevel.NoAccess:
                    return false;

                // access only to assigned user groups, test user groups towards identity
                case UserGroupAccessLevel.OnlyAssigned:
                    var userGroupsMembership =
                        identity.GetUserGrpouMemberships().FirstOrDefault(x => x.TypeId == ((int?)userGroupTypeId ?? 0));
                    var onlyAssignedResult = userGroupsMembership == null
                        ? false
                        : userGroupsMembership.GroupIds.Any(x => x == userGroupId);
                    return onlyAssignedResult;

                // all access - grants access
                case UserGroupAccessLevel.All:
                    return true;

                default:
                    throw new ArgumentOutOfRangeException(nameof(accessLevel), accessLevel,
                        $"EntitleMapper returns unknown UserGroupAccessLevel {accessLevel}.");
            }
        }


        // gets granted user group ids by access level
        private HashSet<long> GetGrantedUserGroupIdsByAccessLevel(UserGroupAccessLevel accessLevel, UserGroupTypeEnum userGroupTypeId, IIdentity identity)
        {
            switch (accessLevel)
            {
                // no access - empty list of granted groups
                case UserGroupAccessLevel.NoAccess:
                    return new HashSet<long>();

                // access only to assigned user groups in identity
                case UserGroupAccessLevel.OnlyAssigned:
                    var userGroupMembership = identity.GetUserGrpouMemberships().FirstOrDefault(x => x.TypeId == (int)userGroupTypeId);
                    return new HashSet<long>(userGroupMembership?.GroupIds ?? new long[0]);

                // all access - null (all is granted)
                case UserGroupAccessLevel.All:
                    return null;

                default:
                    throw new ArgumentOutOfRangeException(nameof(accessLevel), accessLevel,
                        $"EntitleMapper returns unknown UserGroupAccessLevel {accessLevel}.");
            }
        }

        #endregion

    }

}
