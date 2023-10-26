using AutomationSystem.Shared.Core.Emails.Integration.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.System.AsyncRequestExecutors.Models
{
    /// <summary>
    /// Encapsulates composed email messages
    /// </summary>
    public class ComposedEmailMessage
    {
        
        // public properties
        public Email Email { get; set; }
        public EmailMessage EmailMessage { get; set; }

        // constructor       
        public ComposedEmailMessage() { }
        public ComposedEmailMessage(Email email, EmailMessage emailMessage)
        {
            Email = email;
            EmailMessage = emailMessage;
        }
        
    }

}
