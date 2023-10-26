using System.ComponentModel;

namespace AutomationSystem.Shared.Contract.Jobs.AppLogic.Models
{
    public class JobListItem
    {
        [DisplayName("ID")]
        public long JobId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Running interval (minutes)")]
        public int IntervalInMinutes { get; set; }

        [DisplayName("Run from")]
        public string FromHourAndMinute { get; set; }
    }
}
