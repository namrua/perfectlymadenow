using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;

namespace PerfectlyMadeInc.WebEx.Contract.Programs.Models
{
    /// <summary>
    /// Program filter
    /// </summary>
    [Bind(Include = "IncludeUsed")]
    public class ProgramFilter
    {

        // filtering are executed indirectly - userGroup needs to be converted to allowed account ids
        public UserGroupTypeEnum? UserGroupTypeId { get; set; }
        public long? UserGroupId { get; set; }

        // restrictions of the set of user groups
        public List<long> UserGroupIds { get; set; }

        public IEnumerable<long> AllowedAccountsIds { get; set; }

        [DisplayName("Include used")]
        public bool IncludeUsed { get; set; }
        
    }
}
