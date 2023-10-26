using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Identities.System.Models
{
    /// <summary>
    /// User's group membership - !!! keep shrink for claims !!!
    /// </summary>
    public class UserGroupMembership
    {

        public int TypeId { get; set; }
        public long[] GroupIds { get; set; }

        // constructor
        public UserGroupMembership(UserGroupTypeEnum userGroupTypeId, IEnumerable<long> groupIds)
        {
            TypeId = (int)userGroupTypeId;
            GroupIds = groupIds.ToArray();
        }

    }

}
