using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using System.Linq;
using TimeZone = AutomationSystem.Main.Model.TimeZone;

namespace AutomationSystem.Main.Core.Enums.Data.Extensions
{
    public static class EnumFilterExtensions
    {
        
        public static IQueryable<ClassType> Filter(this IQueryable<ClassType> query, EnumItemFilter filter)
        {
            if (filter == null)
                return query;

            if (filter.Id.HasValue)
                query = query.Where(x => x.ClassTypeId == (ClassTypeEnum)filter.Id.Value);
            if (filter.Name != null)
                query = query.Where(x => x.Name == filter.Name);
            return query;
        }
        
        public static IQueryable<Country> Filter(this IQueryable<Country> query, EnumItemFilter filter)
        {
            if (filter == null)
                return query;

            if (filter.Id.HasValue)
                query = query.Where(x => x.CountryId == (CountryEnum)filter.Id.Value);
            if (filter.Name != null)
                query = query.Where(x => x.Name == filter.Name);
            return query;
        }
        
        public static IQueryable<Currency> Filter(this IQueryable<Currency> query, EnumItemFilter filter)
        {
            if (filter == null)
                return query;

            if (filter.Id.HasValue)
                query = query.Where(x => x.CurrencyId == (CurrencyEnum)filter.Id.Value);
            if (filter.Name != null)
                query = query.Where(x => x.Name == filter.Name);
            return query;
        }
        
        public static IQueryable<RegistrationType> Filter(this IQueryable<RegistrationType> query,
            EnumItemFilter filter)
        {
            if (filter == null)
                return query;

            if (filter.Id.HasValue)
                query = query.Where(x => x.RegistrationTypeId == (RegistrationTypeEnum)filter.Id.Value);
            if (filter.Name != null)
                query = query.Where(x => x.Name == filter.Name);
            return query;
        }

        // selects TimeZone entities by filter
        public static IQueryable<TimeZone> Filter(this IQueryable<TimeZone> query, EnumItemFilter filter)
        {
            if (filter == null)
                return query;

            if (filter.Id.HasValue)
                query = query.Where(x => x.TimeZoneId == (TimeZoneEnum)filter.Id.Value);
            if (filter.Name != null)
                query = query.Where(x => x.Name == filter.Name);
            return query;
        }
    }
}
