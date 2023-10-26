using System.Linq;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Emails.Data.Extensions
{
    /// <summary>
    /// Extensions of filters 
    /// TODO: PLEASE EVERYTIME INCLUDE .Active() query
    /// </summary>
    public static class EmailFilterExtensions
    {
        // select EnumTemplate entities by filter, includes restriction on Active items
        public static IQueryable<EmailTemplate> Filter(this IQueryable<EmailTemplate> query, EmailTemplateFilter filter)
        {
            query = query.Active();
            if (filter == null) return query;

            if (filter.EmailTypeId.HasValue)
            {
                query = query.Where(x => x.EmailTypeId == filter.EmailTypeId.Value);
            }

            if (filter.IsDefault.HasValue)
            {
                query = query.Where(x => x.IsDefault == filter.IsDefault.Value);
            }

            if (filter.LanguageId.HasValue)
            {
                query = query.Where(x => x.LanguageId == filter.LanguageId.Value);
            }

            if (filter.EmailTemplateEntityId != null)
            {
                query = query.Where(x => x.EntityTypeId == filter.EmailTemplateEntityId.TypeId && x.EntityId == filter.EmailTemplateEntityId.Id);
            }

            if (filter.IsValidated.HasValue)
            {
                query = query.Where(x => x.IsValidated == filter.IsValidated.Value);
            }

            if (filter.IsSealed.HasValue)
            {
                query = query.Where(x => x.IsSealed == filter.IsSealed.Value);
            }

            return query;
        }
    }
}
