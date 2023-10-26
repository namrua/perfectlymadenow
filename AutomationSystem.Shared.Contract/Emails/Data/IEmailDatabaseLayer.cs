using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Emails.Data
{
    /// <summary>
    /// Database layer for email administration
    /// </summary>
    public interface IEmailDatabaseLayer
    {

        #region email administration

        // gets email types
        List<EmailType> GetEmailTypes();

        // gets email type by id
        EmailType GetEmailTypeById(EmailTypeEnum emailTypeId);

        // get language ids for email type
        HashSet<LanguageEnum> GetLanguageIdsForEmailType(EmailTemplateFilter filter);


        // gets email templates by filter
        List<EmailTemplate> GetEmailTemplatesByFilter(EmailTemplateFilter filter = null, EmailTemplateIncludes includes = EmailTemplateIncludes.None);

        // get default email for type
        EmailTemplate GetDefaultEmailTemplateByType(EmailTypeEnum type, EmailTemplateIncludes includes = EmailTemplateIncludes.None);

        // get email template by id
        EmailTemplate GetEmailTemplateById(long id, EmailTemplateIncludes includes = EmailTemplateIncludes.None);

        EmailTemplate GetEmailTemplateByEmailTemplateEntityId(EmailTemplateEntityId emailTemplateEntityId, EmailTemplateIncludes includes = EmailTemplateIncludes.None);

        // gets template parameters with email parameters by email template id
        List<EmailTemplateParameter> GetEmailTemplateParametersByEmailTemplateId(long emailTemplateId, bool includeEmailParameters = true);

        // get email parameters
        List<EmailParameter> GetEmailParametersByIds(IEnumerable<long> ids);


        // insert email template
        long InsertEmailTemplate(EmailTemplate emailTemplate);       

        // updates email template metadata
        void UpdateEmailTemplateMetadata(EmailTemplate emailTemplate, EmailOperationOption options = EmailOperationOption.None);

        // updates email template text
        void UpdateEmailTemplateText(EmailTemplate emailTemplate, EmailOperationOption options = EmailOperationOption.None);

        // updates email template
        void UpdateEmailTemplate(EmailTemplate template, EmailOperationOption options = EmailOperationOption.None);

        #endregion


        #region email

        // gets email for detail
        Email GetEmailForDetailById(long emailId, EmailIncludes includes = EmailIncludes.None);

        #endregion


        #region email administration integration

        // insert email templates
        void InsertEmailTemplates(IEnumerable<EmailTemplate> emailTemplates);

        // seals email templates
        void SetEmailTemplatesToSealed(IEnumerable<long> emailTemplateIds, EmailOperationOption options = EmailOperationOption.None);

        // delete email template
        void DeleteEmailTemplate(long emailTemplateId, EmailOperationOption options = EmailOperationOption.None);

        // delete email templates by entity id
        void DeleteEmailTemplatesByEntity(EntityTypeEnum entityTypeId, long entityId, EmailOperationOption options = EmailOperationOption.None);

        // gets emails by entity
        List<Email> GetEmailsByEntity(EntityTypeEnum entityTypeId, long entityId, EmailIncludes includes = EmailIncludes.None);

        #endregion


        #region email service

        // gets all email parameters
        List<EmailParameter> GetEmailParameters();

        // saves email
        long SaveEmail(Email email);

        // gets email by id
        Email GetEmailById(long emailId);

        // updates email
        void UpdateEmail(long emailId, bool isSent, DateTime? sent, int sendingAttempts);

        #endregion
        
    }

}
