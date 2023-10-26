using AutomationSystem.Shared.Core.Emails.System.AsyncRequestExecutors.Models;

namespace AutomationSystem.Shared.Core.Emails.System.AsyncRequestExecutors
{
    /// <summary>
    /// Encapsulates database steps of email sending
    /// </summary>
    public interface IEmailSendingDataWrapper
    {

        // gets email message, throws checking exceptions
        ComposedEmailMessage GetEmailMessageByEmailId(long emailId);

        // sends email, throws service exceptions
        void SendEmailMessage(ComposedEmailMessage email);

    }

}
