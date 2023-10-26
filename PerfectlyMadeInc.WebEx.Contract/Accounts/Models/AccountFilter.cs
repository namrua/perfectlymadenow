using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace PerfectlyMadeInc.WebEx.Contract.Accounts.Models
{

    /// <summary>
    /// WebEx account filter
    /// </summary>
    public class AccountFilter
    {

        public UserGroupTypeEnum? UserGroupTypeId { get; set; }
        public long? UserGroupId { get; set; }

        // restrictions of the set of user groups
        public List<long> UserGroupIds { get; set; }

    }
}
