using System;
using System.ComponentModel;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Email list item 
    /// </summary>
    public class EmailListItem
    {

        [DisplayName("ID")]
        public long EmailId { get; set; }

        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Email type")]
        public string EmailType { get; set; }

        [DisplayName("Was sent")]
        public bool IsSent { get; set; }

        [DisplayName("Created")]
        public DateTime Created { get; set; }

        [DisplayName("Sent")]
        public DateTime? Sent { get; set; }

    }

}
