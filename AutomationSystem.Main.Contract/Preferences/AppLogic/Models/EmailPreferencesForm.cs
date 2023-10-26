using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Preferences.AppLogic.Models
{
    /// <summary>
    /// Email form
    /// </summary>
    public class EmailPreferencesForm
    {
        // public properties
        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Administrator email")]
        public string AdminRecipient { get; set; }
    
        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Helpdesk email")]
        public string HelpdeskRecipient { get; set; }       

        [Required]
        [MaxLength(1024)]
        [DisplayName("SendGrid API key")]
        public string SendGridApi { get; set; }

        [Required]
        [MaxLength(64)]
        [DisplayName("Sender name")]
        public string SenderName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Sender email")]
        public string SenderEmail { get; set; }

    }

}
