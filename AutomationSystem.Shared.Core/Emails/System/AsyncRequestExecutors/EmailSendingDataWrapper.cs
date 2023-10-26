using System;
using System.Linq;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Contract.Preferences.System;
using AutomationSystem.Shared.Core.Emails.Integration;
using AutomationSystem.Shared.Core.Emails.Integration.Models;
using AutomationSystem.Shared.Core.Emails.System.AsyncRequestExecutors.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Emails.System.AsyncRequestExecutors
{
    /// <summary>
    /// Encapsulates database steps of email sending
    /// </summary>
    public class EmailSendingDataWrapper : IEmailSendingDataWrapper
    {

         // private components
        private readonly ICorePreferenceProvider preferences;
        private readonly IEmailDatabaseLayer emailDb;
        private readonly ICoreFileService fileService;
        private readonly ITracerFactory tracerFactory;

        private readonly ITracer tracer;

        // constructor
        public EmailSendingDataWrapper(ICorePreferenceProvider preferences, IEmailDatabaseLayer emailDb, ICoreFileService fileService, ITracerFactory tracerFactory)
        {
            this.preferences = preferences;
            this.emailDb = emailDb;
            this.fileService = fileService;
            this.tracerFactory = tracerFactory;
            tracer = tracerFactory.CreateTracer<EmailSendingDataWrapper>();
        }


        // gets email message, throws checking exceptions
        public ComposedEmailMessage GetEmailMessageByEmailId(long emailId)
        {
            // gets email           
            var email = GetEmail(emailId);
            var fileIds = email.EmailAttachments.Select(x => x.FileId).ToList();
            tracer.Info($"Email with id {emailId} was loaded.");

            var message = new EmailMessage();
            message.Subject = email.Subject;
            message.Body = email.Text;
            message.SenderEmail = email.Sender;
            message.SenderName = email.SenderName;
            message.RecipientEmail = email.Recipient;
            message.Attachments = fileService.GetFileForDownloadByIds(fileIds);
            return new ComposedEmailMessage(email, message);
        }

        // sends email, throws service exceptions
        public void SendEmailMessage(ComposedEmailMessage email)
        {
            try
            {
                var sender = GetEmailSender();
                sender.Send(email.EmailMessage).Wait();
                emailDb.UpdateEmail(email.Email.EmailId, true, DateTime.Now, email.Email.SendingAttempts + 1);                
                tracer.Info("Email was sent and updated in database as sent");
            }
            catch (Exception)
            {                
                emailDb.UpdateEmail(email.Email.EmailId, false, null, email.Email.SendingAttempts + 1);
                tracer.Warning("Email was not set, sending attempts was updated");
                throw;
            }
        }


        #region private fields

        // loads email
        private Email GetEmail(long emailId)
        {
            var result = emailDb.GetEmailById(emailId);
            if (result == null)
                throw new ArgumentException($"There is no email with ID {emailId}");
            if (!result.Active)
                throw new InvalidOperationException($"Email with id {emailId} is not active");
            if (result.IsSent)
                throw new InvalidOperationException($"Emailw with id {emailId} is already sent");
            return result;
        }

        // creates sender
        private IEmailSender GetEmailSender()
        {
            var emailSenderSettings = preferences.GetEmailSenderSettings();
            var result = new SendGridEmailSender(emailSenderSettings, tracerFactory);
            return result;
        }

        #endregion

    }

}
