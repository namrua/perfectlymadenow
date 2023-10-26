using System;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.Classes.Data.Extensions
{
    public static class ClassFilterExtensions
    {
        // selects Class entities by filter, includes restriction on Active items
        public static IQueryable<Class> Filter(this IQueryable<Class> query, ClassFilter filter)
        {
            query = query.Active();
            if (filter?.Env == null)
                query = query.Where(x => x.EnvironmentTypeId == EnvironmentTypeEnum.Production);
            if (filter == null)
                return query;

            // environment
            if (filter.Env.HasValue)
                query = query.Where(x => x.EnvironmentTypeId == filter.Env);

            // access privileges
            if (filter.ProfileIds != null)
                query = query.Where(x => filter.ProfileIds.Contains(x.ProfileId));

            // simple states - works only when complex state filters are false
            var nowUtc = DateTime.UtcNow;
            if (!filter.OpenAndCompleted && !filter.OnlyInAfterRegistration)
            {
                query = filter.ClassState.HasValue
                    ? query.FilterByClassState(filter.ClassState, nowUtc)
                    : query.Where(x => !x.IsCanceled && !x.IsFinished);             // only open classes are filtered by default
            }

            // complex state filters
            if (filter.OnlyInAfterRegistration)
                query = query.Where(x =>
                    !x.IsCanceled && !x.IsFinished &&
                    x.RegistrationStartUtc.HasValue && x.RegistrationStartUtc.Value <= nowUtc);
            if (filter.OpenAndCompleted)
                query = query.Where(x => !x.IsCanceled);

            if (filter.CurrencyId.HasValue)
                query = query.Where(x => x.CurrencyId == filter.CurrencyId);

            // categories
            query = filter.ClassCategoryId.HasValue
                ? query.Where(x => x.ClassCategoryId == filter.ClassCategoryId.Value)
                : query.Where(x => x.ClassCategoryId == ClassCategoryEnum.Class || x.ClassCategoryId == ClassCategoryEnum.Lecture);  // only class and lectures are filtered by default

            // other fields
            if (filter.ProfileId.HasValue)
                query = query.Where(x => filter.ProfileId.Value == x.ProfileId);
            if (filter.EventEndUtcFrom.HasValue)
                query = query.Where(x => filter.EventEndUtcFrom.Value < x.EventEndUtc);
            if (filter.EventEndUtcTo.HasValue)
                query = query.Where(x => x.EventEndUtc <= filter.EventEndUtcTo.Value);
            if (filter.NoDetachedHomepage)
                query = query.Where(x => x.ClassStyle.HomepageUrl == null);

            if (filter.IsWwaAllowed.HasValue)
            {
                query = query.Where(x => x.IsWwaFormAllowed);
            }

            return query;
        }

        public static IQueryable<Class> FilterByClassState(this IQueryable<Class> query, ClassState? classState, DateTime? nowUtc = null)
        {
            if (!classState.HasValue)
                return query;

            nowUtc = nowUtc ?? DateTime.UtcNow;

            switch (classState.Value)
            {
                case ClassState.New:
                    query = query.Where(x =>
                        !x.IsCanceled && !x.IsFinished &&
                        (!x.RegistrationStartUtc.HasValue || nowUtc < x.RegistrationStartUtc.Value));
                    break;
                case ClassState.InRegistration:
                    query = query.Where(x =>
                        !x.IsCanceled && !x.IsFinished && x.RegistrationStartUtc.HasValue &&
                        x.RegistrationStartUtc.Value <= nowUtc && nowUtc < x.RegistrationEndUtc);
                    break;
                case ClassState.AfterRegistration:
                    query = query.Where(x =>
                        !x.IsCanceled && !x.IsFinished && x.RegistrationStartUtc.HasValue &&
                        x.RegistrationEndUtc <= nowUtc);
                    break;
                case ClassState.Canceled:
                    query = query.Where(x => x.IsCanceled);
                    break;
                case ClassState.Completed:
                    query = query.Where(x => x.IsFinished);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(classState), classState, "Unknown ClassState.");
            }
            return query;
        }
    }
}
