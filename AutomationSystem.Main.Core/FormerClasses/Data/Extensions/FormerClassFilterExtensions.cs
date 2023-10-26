using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System;
using System.Linq;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;

namespace AutomationSystem.Main.Core.FormerClasses.Data.Extensions
{
    public static class FormerClassFilterExtensions
    {
        public static IQueryable<FormerClass> Filter(this IQueryable<FormerClass> query, FormerClassFilter filter)
        {
            query = query.Active();
            if (filter == null) return query;

            // if not null, synonymous class types is used instead of ClassTypeId
            if (filter.SynonymousClassTypes != null)
            {
                query = query.Where(x => filter.SynonymousClassTypes.Contains(x.ClassTypeId));
            }
            else if (filter.ClassTypeId.HasValue)
            {
                query = query.Where(x => x.ClassTypeId == filter.ClassTypeId);
            }

            if (!string.IsNullOrEmpty(filter.Location))
            {
                query = query.Where(x => x.Location.ToLower().Contains(filter.Location.ToLower()));
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(x => filter.FromDate <= x.EventStart);
            }

            var toDateUpperBound = GetNextDayStart(filter.ToDate);
            if (toDateUpperBound.HasValue)
            {
                query = query.Where(x => x.EventEnd < toDateUpperBound);
            }

            if (filter.ProfileId.HasValue)
            {
                query = query.Where(x => x.ProfileId == filter.ProfileId);
            }

            return query;
        }

        #region private method

        // returns next day at 0:00:00 for sharp comparation
        private static DateTime? GetNextDayStart(DateTime? dateTime)
        {
            if (!dateTime.HasValue) return null;
            var dt = dateTime.Value;
            var result = new DateTime(dt.Year, dt.Month, dt.Day);
            result = result.AddDays(1);
            return result;
        }

        #endregion
    }
}
