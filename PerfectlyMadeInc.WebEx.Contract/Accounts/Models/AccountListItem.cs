using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace PerfectlyMadeInc.WebEx.Contract.Accounts.Models
{
    /// <summary>
    /// WebEx model for list
    /// </summary>
    public class AccountListItem
    {

        public long ConferenceAccountId { get; set; }
        public long AccountId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Site name")]
        public string SiteName { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }

        [DisplayName("Is active")]
        public bool Active { get; set; }

        [DisplayName("User group type code")]
        public UserGroupTypeEnum? UserGroupTypeId { get; set; }

        [DisplayName("User group ID")]
        public long? UserGroupId { get; set; }

    }
}
