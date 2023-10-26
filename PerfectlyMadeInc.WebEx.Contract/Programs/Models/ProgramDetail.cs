using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;

namespace PerfectlyMadeInc.WebEx.Contract.Programs.Models
{
    /// <summary>
    /// Program detail
    /// </summary>
    public class ProgramDetail
    {
        [HiddenInput]
        public long Id { get; set; }

        [DisplayName("ID")]
        public long ProgramOuterId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Program URL")]
        public string ProgramUrl { get; set; }

        [DisplayName("Is used")]
        public bool IsUsed { get; set; }

        [DisplayName("Account")]
        public string AccountName { get; set; }

        [DisplayName("User group type code")]
        public UserGroupTypeEnum? UserGroupTypeId { get; set; }

        [DisplayName("User group ID")]
        public long? UserGroupId { get; set; }

        [DisplayName("Events")]
        public List<EventListItem> Events { get; set; } = new List<EventListItem>();

    }
}
