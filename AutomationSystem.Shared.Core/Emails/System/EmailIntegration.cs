using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.AppLogic.Convertors;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.System
{
    /// <summary>
    /// Integrates email templates administration to other pages
    /// </summary>
    public class EmailIntegration : IEmailIntegration
    {

        private readonly IEmailDatabaseLayer emailDb;
        private readonly IEmailTemplateConvertor templateConvertor;
        private readonly IEmailConvertor emailConvertor;
        private readonly IEmailTemplateResolver resolver;


        // constructor
        public EmailIntegration(
            IEmailDatabaseLayer emailDb,
            IEmailTemplateConvertor templateConvertor,
            IEmailConvertor emailConvertor,
            IEmailTemplateResolver resolver)
        {
            this.emailDb = emailDb;
            this.templateConvertor = templateConvertor;
            this.emailConvertor = emailConvertor;
            this.resolver = resolver;
        }


        // return clones of email template for given email type and languages (executes check)
        public List<EmailTemplate> CloneEmailTemplates(EmailTypeEnum emailTypeId, EmailTemplateEntityId emailTemplateEntityId, params LanguageEnum?[] languageIds)
        {
            var languages = new HashSet<LanguageEnum>(languageIds.Where(x => x.HasValue).Select(x => x.Value));
            var templates = resolver.GetValidTemplates(emailTemplateEntityId, emailTypeId, languages, EmailTemplateIncludes.EmailTemplateParameter);

            var missingLanguage = new HashSet<LanguageEnum>(languages);
            missingLanguage.ExceptWith(templates.Select(x => x.LanguageId));

            // process result
            if (missingLanguage.Count > 0)
            {
                throw new ArgumentException($"There are no valid email templates of type {emailTypeId} for languages: {string.Join(", ", missingLanguage.Select(x => x.ToString()))}.");
            }

            return templates.Select(x => templateConvertor.CloneEmailTemplate(x)).ToList();
        }


        // saves clones of email templates for given entity type id and entity id
        public void SaveClonedEmailTemplates(List<EmailTemplate> emailTemplates, EmailTemplateEntityId emailTemplateEntityId)
        {
            foreach (var emailTemplate in emailTemplates)
            {
                emailTemplate.EntityTypeId = emailTemplateEntityId.TypeId;
                emailTemplate.EntityId = emailTemplateEntityId.Id;
            }
            emailDb.InsertEmailTemplates(emailTemplates);
        }


        // gets email templates list item for given entity
        public List<EmailTemplateListItem> GetEmailTemplateListItemsByEntity(EmailTemplateEntityId emailTemplateEntityId, bool onlyNotSealed = false)
        {
            // loads EmailTemplates
            var emailTemplateFilter = new EmailTemplateFilter
            {
                IsDefault = false,
                IsValidated = true,
                EmailTemplateEntityId = emailTemplateEntityId,
                IsSealed = onlyNotSealed ? (bool?)false : null
            };
            var templates = emailDb.GetEmailTemplatesByFilter(emailTemplateFilter, EmailTemplateIncludes.Language);
            var result = templates.Select(templateConvertor.ConvertToEmailTemplateListItem).ToList();
            return result;
        }

        // gets emails for given entity
        public List<EmailListItem> GetEmailsByEntity(EmailEntityId emailEntityId)
        {
            var emails = emailDb.GetEmailsByEntity(emailEntityId.TypeId, emailEntityId.Id, EmailIncludes.EmailTemplateEmailType);
            var result = emails.Select(emailConvertor.ConvertToEmailListItem).ToList();
            return result;
        }


        // delete email template
        public void DeleteEmailTemplate(long emailTemplateId)
        {
            emailDb.DeleteEmailTemplate(emailTemplateId, EmailOperationOption.CheckNoEmails);
        }


        // delete email templates by entity id
        public void DeleteEmailTemplatesByEntity(EmailEntityId emailEntityId)
        {
            emailDb.DeleteEmailTemplatesByEntity(emailEntityId.TypeId, emailEntityId.Id, EmailOperationOption.CheckNoEmails);
        }

    }
    
}
