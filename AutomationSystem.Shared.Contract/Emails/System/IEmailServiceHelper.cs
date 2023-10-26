using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    /// <summary>
    /// Email service helper - encapsulates common functionality
    /// </summary>
    public interface IEmailServiceHelper
    {
        // gets parameter definitions
        List<EmailParameter> EmailParameters { get; }

        // gets parameter convertor
        IEmailParameterConvertor EmailParameterConvertor { get; }


        // gets language info provider
        ILanguageInfoPovider LanguageInfoProvider { get; }

        // gets localisation service
        ILocalisationService LocalisationService { get; }

        // gets localised enum item
        IEnumItem GetLocalisedEnumItem<T>(LanguageInfo languageInfo, EnumTypeEnum enumTypeId, T itemId);

        // gets localised text
        string GetLocalisedText(LanguageInfo languageInfo, string label, string module = "EmailTemplate");


        // gets admin recipient
        string GetAdminRecipient();

        // gets helpdesk recipient
        string GetHelpdeskEmail();               


        // sends test email
        long SendEmail(long emailTemplateId, EmailEntityId emailEntityId, bool isTest, string subject,
            string text, string recipient, SenderInfo senderInfo, int severity, IEmailAttachmentProvider attachments = null);

        // send email for email template
        long SendEmailForTemplate(EmailTemplate template, IEmailTextResolver textResolver, EmailEntityId emailEntityId,
            string recipient, SenderInfo senderInfo, int severity, IEmailAttachmentProvider attachments = null);

    }

}
