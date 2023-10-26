using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Base.Contract.Identities.Models
{

    /// <summary>
    /// Encapsulates granted user groups for given entitle and user group type
    /// </summary>
    public class UserGroupsForEntitle
    {

        public Entitle Entitle { get; }
        public UserGroupTypeEnum UserGroupTypeId { get; }


        // determines maximal user group access level
        public UserGroupAccessLevel UserGroupAccessLevel { get; set; }

        // null = all groups granted, empty = no group granted, [*,*] = listed groups granted
        public HashSet<long> GrantedUserGroupIds { get; set; }

        // default user group (id = null) is also granted
        public bool IncludeDefaultGroup { get; set; }


        // constructor
        public UserGroupsForEntitle(Entitle entitle, UserGroupTypeEnum userGroupTypeId)
        {
            Entitle = entitle;
            UserGroupTypeId = userGroupTypeId;
        }

    }
}
