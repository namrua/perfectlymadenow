using System.Linq;
using AutomationSystem.Shared.Core.Localisation.Data.Models;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Localisation.Data.Extensions
{
    /// <summary>
    /// Extensions of filters 
    /// TODO: PLEASE EVERYTIME INCLUDE .Active() query
    /// </summary>
    public static class LocalisationFilterExtensions
    {
        // selects AppLocalisation entities by filter, includes restriction on Active items
        public static IQueryable<AppLocalisation> Filter(this IQueryable<AppLocalisation> query, AppLocalisationFilter filter)
        {
            query = query.Active();
            if (filter == null) return query;

            if (filter.SupportedLanguages != null)
                query = query.Where(x => filter.SupportedLanguages.Contains(x.Language.Name));
            if (filter.LanguageId.HasValue)
                query = query.Where(x => x.LanguageId == filter.LanguageId.Value);
            if (filter.Module != null)
                query = query.Where(x => x.Module == filter.Module);
            if (filter.Label != null)
                query = query.Where(x => x.Label == filter.Label);
            return query;
        }

        // selecte DataLocalisation entities by filter, includes restriction on Active items
        public static IQueryable<DataLocalisation> Filter(this IQueryable<DataLocalisation> query,
            DataLocalisationFilter filter)
        {
            query = query.Active();
            if (filter == null) return query;

            if (filter.EntityIds != null)
                query = query.Where(x => filter.EntityIds.Contains(x.EntityId));
            if (filter.LanguageId.HasValue)
                query = query.Where(x => x.LanguageId == filter.LanguageId.Value);
            if (filter.LanguageCode != null)
                query = query.Where(x => x.Language.Name == filter.LanguageCode);
            if (filter.EntityType.HasValue)
                query = query.Where(x => x.EntityTypeId == filter.EntityType.Value);
            return query;
        }

        // selects EnumLocalisation entities by filter, includes restriction on Active items
        public static IQueryable<EnumLocalisation> Filter(
            this IQueryable<EnumLocalisation> query,
            EnumLocalisationFilter filter)
        {
            query = query.Active();
            if (filter == null) return query;

            if (filter.ItemId.HasValue)
                query = query.Where(x => x.ItemId == filter.ItemId.Value);
            if (filter.ItemIds != null)
                query = query.Where(x => filter.ItemIds.Contains(x.ItemId));
            if (filter.LanguageId.HasValue)
                query = query.Where(x => x.LanguageId == filter.LanguageId.Value);
            if (filter.LanguageCode != null)
                query = query.Where(x => x.Language.Name == filter.LanguageCode);
            if (filter.EnumTypeId.HasValue)
                query = query.Where(x => x.EnumTypeId == filter.EnumTypeId.Value);

            return query;
        }
    }
}
