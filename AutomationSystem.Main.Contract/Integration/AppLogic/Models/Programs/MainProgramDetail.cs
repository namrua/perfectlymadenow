using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using PerfectlyMadeInc.WebEx.Contract.Programs.Models;

namespace AutomationSystem.Main.Contract.Integration.AppLogic.Models.Programs
{
    /// <summary>
    /// Main Program detail
    /// </summary>
    public class MainProgramDetail
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

        [DisplayName("Profile")]
        public string Profile { get; set; }

        [DisplayName("Events")]
        public List<EventListItem> Events { get; set; } = new List<EventListItem>();

    }


}
