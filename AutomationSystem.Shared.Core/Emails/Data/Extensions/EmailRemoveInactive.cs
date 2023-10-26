using System.Linq;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Emails.Data.Extensions
{
    /// <summary>
    /// Aggregates include extensions
    /// </summary>
    public static class EmailRemoveInactive
    {
        // removes inactive includes for EmailTemplate
        public static EmailTemplate RemoveInactiveForEmailTemplate(EmailTemplate entity, EmailTemplateIncludes includes)
        {
            if (entity == null)
                return null;
            if (includes.HasFlag(EmailTemplateIncludes.EmailTemplateParameter))
                entity.EmailTemplateParameters = entity.EmailTemplateParameters.AsQueryable().Active().ToList();
            return entity;
        }
    }
}
