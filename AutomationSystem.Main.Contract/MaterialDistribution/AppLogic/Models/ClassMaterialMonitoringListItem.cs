using System;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Encapsulates class material monitoring list item related to one recipient
    /// </summary>
    public class ClassMaterialMonitoringListItem
    {
        [DisplayName("ID")]
        public long? ClassMaterialRecipientId { get; set; }

        [DisplayName("Recipient ID")]
        public RecipientId RecipientId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Student's password")]
        public string Password { get; set; }

        [DisplayName("Request code")]
        public string RequestCode { get; set; }

        [DisplayName("Total downloads")]
        public int TotalDonwnloadCount { get; set; }

        [DisplayName("Last notification")]
        public DateTime? Notified { get; set; }
    }
}
