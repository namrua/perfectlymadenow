namespace AutomationSystem.Shared.Contract.Emails.Integration.Models
{
    /// <summary>
    /// SendGrid service settings
    /// </summary>
    public class EmailSenderSettings
    {

        // public properties
        public string SendGridApi { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
