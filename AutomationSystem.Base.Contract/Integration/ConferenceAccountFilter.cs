using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Base.Contract.Integration
{
    /// <summary>
    /// ConferenceAccount filter
    /// </summary>
    public class ConferenceAccountFilter
    {
        public ConferenceAccountTypeEnum? ConferenceAccountTypeId { get; set; }
        public UserGroupTypeEnum? UserGroupTypeId { get; set; }
        public long? UserGroupId { get; set; }

        // restrictions of the set of user groups
        public List<long> UserGroupIds { get; set; }

    }

}
