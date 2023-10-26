using System.Threading.Tasks;
using AutomationSystem.Shared.Core.Emails.Integration.Models;

namespace AutomationSystem.Shared.Core.Emails.Integration
{
    /// <summary>
    /// Encapsulates email sending
    /// </summary>
    public interface IEmailSender
    {

        // sends email message
        Task Send(EmailMessage emailMessage);

    }

}
