using System.Data.Entity.Infrastructure;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.Data.Extensions
{
    /// <summary>
    /// Aggregates include extensions
    /// </summary>
    public static class EmailIncludeExtensions
    {
        // adds includes for Email
        public static DbQuery<Email> AddIncludes(this DbQuery<Email> query, EmailIncludes includes)
        {
            if (includes.HasFlag(EmailIncludes.EmailTemplate))
            {
                query = query.Include("EmailTemplate");
            }

            if (includes.HasFlag(EmailIncludes.EmailTemplateEmailType))
            {
                query = query.Include("EmailTemplate.EmailType");
            }

            if (includes.HasFlag(EmailIncludes.EntityType))
            {
                query = query.Include("EntityType");
            }

            return query;
        }


        // adds includes for EmailTemplate
        public static DbQuery<EmailTemplate> AddIncludes(this DbQuery<EmailTemplate> query, EmailTemplateIncludes includes)
        {
            if (includes.HasFlag(EmailTemplateIncludes.Language))
            {
                query = query.Include("Language");
            }

            if (includes.HasFlag(EmailTemplateIncludes.EmailType))
            {
                query = query.Include("EmailType");
            }

            if (includes.HasFlag(EmailTemplateIncludes.EmailTemplateParameter))
            {
                query = query.Include("EmailTemplateParameters");
            }

            return query;
        }
    }
}
