using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using AutomationSystem.Shared.Contract.Emails.Integration.Models;
using AutomationSystem.Shared.Contract.Files.System.Models;
using AutomationSystem.Shared.Core.Emails.Integration.Models;
using CorabeuControl.Helpers;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AutomationSystem.Shared.Core.Emails.Integration
{
    /// <summary>
    /// Encapsulates email sending
    /// </summary>
    public class SendGridEmailSender : IEmailSender
    {

        // private fields
        private readonly EmailSenderSettings settings;
        private readonly ITracer tracer;

        // constructor
        public SendGridEmailSender(EmailSenderSettings settings, ITracerFactory tracerFactory)
        {
            this.settings = settings;
            tracer = tracerFactory.CreateTracer<SendGridEmailSender>(settings.SenderEmail);
        }


        // sends email message
        public async Task Send(EmailMessage emailMessage)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(emailMessage.SenderEmail ?? settings.SenderEmail, emailMessage.SenderName ?? settings.SenderName));
            if (emailMessage.SenderEmail != null && emailMessage.SenderName != null)
            {
                msg.SetReplyTo(new EmailAddress(emailMessage.SenderEmail, emailMessage.SenderName));
            }
            msg.AddTo(new EmailAddress(emailMessage.RecipientEmail));           
            msg.SetSubject(emailMessage.Subject);
            msg.AddContent(MimeType.Html, TextHelper.ReplaceNewLinesString(emailMessage.Body));    
            msg.AddContent(MimeType.Text, emailMessage.Body);

            // adds attachments - warning: AddAttachments with 0 attachments causes that email is not sent
            var attachments = emailMessage.Attachments.Select(ConvertToAttachment).ToList(); 
            if (attachments.Any())
                msg.AddAttachments(attachments);
           
            // disabling tracking settings
            msg.TrackingSettings = new TrackingSettings();
            msg.TrackingSettings.ClickTracking = new ClickTracking();
            msg.TrackingSettings.ClickTracking.Enable = false;
            msg.TrackingSettings.ClickTracking.EnableText = false;
            
            // creates client and sends email
            var client = new SendGridClient(settings.SendGridApi);
            var response = await client.SendEmailAsync(msg);

            // if response status code is not Accepted, throws exception
            if (response.StatusCode != HttpStatusCode.Accepted)
                throw new HttpException($"Response status code of SendGrid request is not 'Accepted' ({response.StatusCode}).");

            // logs result
            tracer.Info($"Email send; response: statusCode = {response.StatusCode}; header = {response.Headers}, body = {await response.Body.ReadAsStringAsync()}");            
        }


        #region private methods

        // converts file for download to attachment object
        private Attachment ConvertToAttachment(FileForDownload file)
        {
            var result = new Attachment
            {
                Type = file.MimeType,
                Filename = file.FileName,
                Disposition = "attachment",
                Content = Convert.ToBase64String(file.Content)
            };           
            return result;
        }

        #endregion

    }

} 
