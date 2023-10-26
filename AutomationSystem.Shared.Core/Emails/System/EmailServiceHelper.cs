using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Contract.Preferences.System;
using AutomationSystem.Shared.Core.Emails.System.EmailAttachmentProviders;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.System
{
    /// <summary>
    /// Email service helper - encapsulates common functionality
    /// </summary>
    public class EmailServiceHelper : IEmailServiceHelper
    {

        // private components
        private readonly IEmailDatabaseLayer emailDb;      
        private readonly ICorePreferenceProvider corePreferenceProvider;
        private readonly ILocalisationService localisationService;
        private readonly ICoreAsyncRequestManager asyncRequestManager;

        // lazy property fields
        private readonly Lazy<List<EmailParameter>> parameters;
        private readonly Lazy<ILanguageInfoPovider> languageInfoProvider;
        private readonly Lazy<IEmailParameterConvertor> parameterConvertor;

        // inner components
        private readonly IEmailAttachmentProvider emptyAttachmentProvider;

        // constructor
        public EmailServiceHelper(IEmailDatabaseLayer emailDb, ICorePreferenceProvider corePreferenceProvider, 
            ILocalisationService localisationService, ICoreAsyncRequestManager asyncRequestManager)
        {
            this.emailDb = emailDb;           
            this.corePreferenceProvider = corePreferenceProvider;
            this.localisationService = localisationService;
            this.asyncRequestManager = asyncRequestManager;

            parameters = new Lazy<List<EmailParameter>>(emailDb.GetEmailParameters);
            languageInfoProvider = new Lazy<ILanguageInfoPovider>(localisationService.GetLanguageInfoProvider);
            parameterConvertor = new Lazy<IEmailParameterConvertor>(() => new EmailParameterConvertor(EmailParameters, LanguageInfoProvider));
            emptyAttachmentProvider = new SimpleEmailAttachmentProvider();
        }


        #region lazy properties

        // gets parameter definitions
        public List<EmailParameter> EmailParameters => parameters.Value;

        // gets parameter convertor
        public IEmailParameterConvertor EmailParameterConvertor => parameterConvertor.Value;

        // gets language info provider
        public ILanguageInfoPovider LanguageInfoProvider => languageInfoProvider.Value;

        // gets localisation service
        public ILocalisationService LocalisationService => localisationService;

        #endregion


        #region localisation helpers

        // gets localised enum item
        public IEnumItem GetLocalisedEnumItem<T>(LanguageInfo languageInfo, EnumTypeEnum enumTypeId, T itemId)
        {
            var intItemId = (int)(object)itemId;
            var result = LocalisationService.GetLocalisedEnumItemSpecified(enumTypeId, intItemId, languageInfo.Name);
            return result;
        }

        // gets localised text
        public string GetLocalisedText(LanguageInfo languageInfo, string label, string module = "EmailTemplate")
        {
            var result = LocalisationService.GetLocalisedString(module, label, languageInfo.Name);
            return result;
        }

        #endregion


        // gets admin recipient
        public string GetAdminRecipient()
        {
            return corePreferenceProvider.GetAdminRecipient();
        }


        // gets helpdesk recipient
        public string GetHelpdeskEmail()
        {
            return corePreferenceProvider.GetHelpdeskEmail();
        }

                    
        // sends test email
        public long SendEmail(long emailTemplateId, EmailEntityId emailEntityId, bool isTest,
            string subject, string text, string recipient, SenderInfo senderInfo, int severity, IEmailAttachmentProvider attachments = null)
        {
            // assembles email
            var email = new Email();
            email.EmailTemplateId = emailTemplateId;
            email.EntityTypeId = emailEntityId.TypeId;
            email.EntityId = emailEntityId.Id;
            email.IsTestEmail = isTest;
            email.Subject = subject;
            email.Text = text;
            email.Recipient = recipient;
            email.Sender = senderInfo?.SenderEmail;
            email.SenderName = senderInfo?.SenderName;
            email.Severity = severity;
            email.Active = true;
            email.SendingAttempts = 0;
            email.IsHtml = false;

            // adds attachments
            var fileIdsToAttachment = attachments?.GetFileIdsByEntity(emailEntityId.TypeId, emailEntityId.Id) ?? new List<long>();
            foreach (var fileIdToAttachment in fileIdsToAttachment)
            {
                var emailAttachment = new EmailAttachment { FileId = fileIdToAttachment };
                email.EmailAttachments.Add(emailAttachment);
            }

            // saves to db
            var result = emailDb.SaveEmail(email);

            // adds async request
            if (severity <= (int)SeverityEnum.Normal)
                asyncRequestManager.AddSendEmailRequest(result, severity);
            return result;
        }


        // send email for email template
        public long SendEmailForTemplate(EmailTemplate emailTemplate, IEmailTextResolver textResolver, EmailEntityId emailEntityId,
            string recipient, SenderInfo senderInfo, int severity, IEmailAttachmentProvider attachments = null)
        {
            // resolves inputs
            var subject = textResolver.GetText(emailTemplate.LanguageId, emailTemplate.Subject);
            var text = textResolver.GetText(emailTemplate.LanguageId, emailTemplate.Text);

            // sends email
            var result = SendEmail(emailTemplate.EmailTemplateId, emailEntityId, false, subject, text, recipient, senderInfo, severity, attachments);
            return result;
        }

    }

}
