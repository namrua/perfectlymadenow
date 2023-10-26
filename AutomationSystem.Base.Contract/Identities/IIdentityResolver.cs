using System.Collections.Generic;
using System.Security.Principal;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models;

namespace AutomationSystem.Base.Contract.Identities
{

    /// <summary>
    ///  Grants entitles and user groups by user identity
    /// </summary>
    public interface IIdentityResolver
    {
        // returns current IIdentity
        IIdentity GetCurrentIdentity();

        // resolves owner ID
        int GetOwnerId();

        // resolves whether entitle is granted
        bool IsEntitleGranted(Entitle entitle);

        // checks whether Entitle is granted, if no - throws EntitleAccessDeniedException
        void CheckEntitle(Entitle entitle);

        // resolves whether any entitle is granted
        bool IsAnyEntitleGranted(HashSet<Entitle> entitles);

        // resolves whether entitle for entity in usergroup is granted
        bool IsEntitleGrantedForUserGroup(Entitle entitle, UserGroupTypeEnum? userGroupTypeId, long? userGroupId);

        // checks whether Entitle is granted for user group, if no - throws EntitleAccessDeniedException
        void CheckEntitleForUserGroup(Entitle entitle, UserGroupTypeEnum? userGroupTypeId, long? userGroupId,
            EntityTypeEnum? entityTypeId = null, long? entityId = null, string entityType = null);

        // gets granted user groups for given entitle and user group type
        UserGroupsForEntitle GetGrantedUserGroupsForEntitle(Entitle entitle, UserGroupTypeEnum userGroupTypeId);

    }
}
