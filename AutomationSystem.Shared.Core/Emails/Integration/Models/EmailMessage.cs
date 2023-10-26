using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Shared.Core.Emails.Integration.Models
{
    /// <summary>
    /// The class encapsulates email message informations
    /// </summary>
    public class EmailMessage
    {

        // public properties        
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string RecipientEmail { get; set; }     
        public string Subject { get; set; }
        public string Body { get; set; }        
        public List<FileForDownload> Attachments { get; set; }


        // constructor
        public EmailMessage()
        {
            Attachments = new List<FileForDownload>();
        }

    }


}
