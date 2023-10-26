using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Shared.Core.Emails.System
{
    public class EmailTemplateResolver : IEmailTemplateResolver
    {
        private readonly IEmailTemplateHierarchyResolver resolver;
        private readonly IEmailDatabaseLayer emailDb;

        public EmailTemplateResolver(IEmailTemplateHierarchyResolver resolver, IEmailDatabaseLayer emailDb)
        {
            this.resolver = resolver;
            this.emailDb = emailDb;
        }

        public EmailTemplate GetParentTemplate(
            EmailTemplateEntityId emailTemplateEntityId,
            LanguageEnum languageId,
            EmailTypeEnum emailTypeId,
            EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            var entityHierarchy = resolver.GetHierarchyForParent(emailTemplateEntityId);
            entityHierarchy.Entities.Reverse();
            foreach (var entity in entityHierarchy.Entities)
            {
                var filter = new EmailTemplateFilter(emailTypeId)
                {
                    LanguageId = languageId,
                    IsDefault = false,
                    EmailTemplateEntityId = entity
                };
                var template = emailDb.GetEmailTemplatesByFilter(filter, includes).FirstOrDefault();
                if (template != null)
                {
                    return template;
                }
            }

            if (entityHierarchy.CanUseDefault)
            {
                var defaultTemplate = emailDb.GetDefaultEmailTemplateByType(emailTypeId, includes);
                if (defaultTemplate != null)
                {
                    return defaultTemplate;
                }
            }

            throw new InvalidOperationException($"There is no parent email template for email type {emailTypeId}, language {languageId} and entity {emailTemplateEntityId}.");
        }

        public EmailTemplate GetParentTemplate(EmailTemplate template, EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            return GetParentTemplate(new EmailTemplateEntityId(template.EntityTypeId, template.EntityId), template.LanguageId, template.EmailTypeId, includes);
        }

        public List<EmailTemplate> GetValidTemplates(
            EmailTemplateEntityId emailTemplateEntityId,
            EmailTypeEnum emailTypeId,
            HashSet<LanguageEnum> languageIds,
            EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            var entityHierarchy = resolver.GetHierarchy(emailTemplateEntityId);
            entityHierarchy.Entities.Reverse();
            var validTemplates = new List<EmailTemplate>();
            var missingLanguages = new HashSet<LanguageEnum>(languageIds);
            foreach (var entity in entityHierarchy.Entities)
            {
                if (missingLanguages.Count == 0)
                {
                    break;
                }

                var filter = new EmailTemplateFilter(emailTypeId)
                {
                    IsValidated = true,
                    IsDefault = false,
                    EmailTemplateEntityId = entity
                };

                var templates = emailDb.GetEmailTemplatesByFilter(filter, includes).Where(x => missingLanguages.Contains(x.LanguageId));
                validTemplates.AddRange(templates);
                missingLanguages.ExceptWith(validTemplates.Select(x => x.LanguageId));
            }

            return validTemplates;
        }

        public EmailTemplate GetValidTemplate(
            EmailTypeEnum emailTypeId,
            LanguageEnum languageId,
            EmailTemplateEntityId emailTemplateEntityId,
            EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            var result = GetValidTemplates(emailTemplateEntityId, emailTypeId, new HashSet<LanguageEnum>{languageId}, includes).FirstOrDefault();
            if (result == null)
            {
                throw new ArgumentException($"There is no email template for email type {emailTypeId}.");
            }

            return result;
        }

        public EmailTemplate GetEmailTemplateByEmailTemplateEntityId(EmailTemplateEntityId emailTemplateEntityId, EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            var result = emailDb.GetEmailTemplateByEmailTemplateEntityId(emailTemplateEntityId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no valid email template for emailTemplateEntityId {emailTemplateEntityId}.");
            }

            return result;
        }

        public EmailTemplate GetEmailTemplateById(long id, bool allowInvalidTemplate = false)
        {
            // loads template data
            var result = emailDb.GetEmailTemplateById(id);
            if (result == null || (!allowInvalidTemplate && !result.IsValidated))
                throw new ArgumentException($"There is no valid email template with id {id}.");
            return result;
        }
        
        public EmailTemplate GetEmailTemplateByTypeAndLanguage(EmailTypeEnum emailTypeId,
            LanguageEnum language = LocalisationInfo.DefaultLanguage, bool canUseDefault = false)
        {
            // creates filter
            var templateFilter = new EmailTemplateFilter(emailTypeId)
            {
                IsDefault = canUseDefault ? (bool?)null : false,
                IsValidated = true,
                EmailTemplateEntityId = new EmailTemplateEntityId(),
                LanguageId = language
            };

            // loads email template
            var templates = emailDb.GetEmailTemplatesByFilter(templateFilter);
            var result = templates.FirstOrDefault(x => !x.IsDefault);
            if (canUseDefault && result == null)
                result = templates.FirstOrDefault();
            if (result == null)
                throw new ArgumentException($"There is no valid system email template with type {emailTypeId} and language {language}.");
            return result;
        }
    }
}
