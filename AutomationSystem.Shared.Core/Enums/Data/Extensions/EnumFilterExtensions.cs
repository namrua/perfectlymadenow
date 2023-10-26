using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Enums.Data.Extensions
{
    /// <summary>
    /// Extensions of filters 
    /// TODO: PLEASE EVERYTIME INCLUDE .Active() query
    /// </summary>
    public static class EnumFilterExtensions
    {
        // selects Language entities by filter
        public static IQueryable<Language> Filter(this IQueryable<Language> query, EnumItemFilter filter)
        {
            if (filter == null)
                return query;

            if (filter.Id.HasValue)
                query = query.Where(x => x.LanguageId == (LanguageEnum)filter.Id.Value);
            if (filter.Name != null)
                query = query.Where(x => x.Name == filter.Name);
            return query;
        }
    }
}
