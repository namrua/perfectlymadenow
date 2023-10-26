using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace PerfectlyMadeInc.WebEx.Contract.Programs.Models
{
    /// <summary>
    /// Program list item
    /// </summary>
    public class ProgramListItem
    {
        public long Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Account")]
        public string AccountName { get; set; }

        [DisplayName("User group type code")]
        public UserGroupTypeEnum? UserGroupTypeId { get; set; }

        [DisplayName("User group ID")]
        public long? UserGroupId { get; set; }

    }
}
