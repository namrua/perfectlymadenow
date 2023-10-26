namespace AutomationSystem.Shared.Contract.Emails.System.Models
{
    /// <summary>
    /// Encapsulates informations about sender
    /// </summary>
    public class SenderInfo
    {

        public string SenderEmail { get; set; }
        public string SenderName { get; set; }

        // constructor
        public SenderInfo() {}

        public SenderInfo(string senderEmail, string senderName)
        {
            SenderEmail = senderEmail;
            SenderName = senderName;
        }

    }

}
