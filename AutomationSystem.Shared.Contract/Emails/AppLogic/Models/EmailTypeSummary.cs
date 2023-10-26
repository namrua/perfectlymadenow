using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Email type summary
    /// </summary>
    public class EmailTypeSummary
    {
        public EmailTemplateEntityId EmailTemplateEntityId { get; set; }
      
        public List<EmailTypeSummaryItem> Items { get; set; }

        // constructor
        public EmailTypeSummary()
        {
            Items = new List<EmailTypeSummaryItem>();
        }

    }
}
