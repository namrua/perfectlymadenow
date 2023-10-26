using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System;
using System.Linq;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents;

namespace AutomationSystem.Main.Core.FormerClasses.Data.Extensions
{
    public static class FormerStudentFilterExtensions
    {
        public static IQueryable<FormerStudent> Filter(this IQueryable<FormerStudent> query, FormerStudentFilter filter)
        {
            query = query.Active();
            if (filter == null) return query;

            // contact
            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(x => x.Email.ToLower().Contains(filter.Email.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Phone))
            {
                query = query.Where(x => x.Phone.ToLower().Contains(filter.Phone.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Contact))
            {
                query = query.Where(x => x.Email.ToLower().Contains(filter.Contact.ToLower())
                                         || x.Phone.ToLower().Contains(filter.Contact.ToLower()));
            }

            // queries address
            if (filter.Address != null)
            {
                var adrFilter = filter.Address;
                if (!string.IsNullOrEmpty(adrFilter.FirstName))
                    query = query.Where(x => x.Address.FirstName.ToLower().Contains(adrFilter.FirstName.ToLower()));
                if (!string.IsNullOrEmpty(adrFilter.LastName))
                    query = query.Where(x => x.Address.LastName.ToLower().Contains(adrFilter.LastName.ToLower()));
                if (!string.IsNullOrEmpty(adrFilter.Street))
                    query = query.Where(x => x.Address.Street.ToLower().Contains(adrFilter.Street.ToLower())
                                             || x.Address.Street2.ToLower().Contains(adrFilter.Street.ToLower()));
                if (!string.IsNullOrEmpty(adrFilter.CityState))
                    query = query.Where(x => x.Address.City.ToLower().Contains(adrFilter.CityState.ToLower())
                                             || x.Address.State.ToLower().Contains(adrFilter.CityState.ToLower()));
                if (adrFilter.CountryId.HasValue)
                    query = query.Where(x => x.Address.CountryId == adrFilter.CountryId);
            }

            // queries class id
            if (filter.FormerClassId.HasValue)
                query = query.Where(x => x.FormerClassId == filter.FormerClassId.Value);

            // query class
            if (filter.Class != null)
            {
                var clsFilter = filter.Class;
                if (clsFilter.FromDate.HasValue)
                    query = query.Where(x => clsFilter.FromDate <= x.FormerClass.EventStart);
                var toDateUpperBound = GetNextDayStart(filter.Class.ToDate);
                if (toDateUpperBound.HasValue)
                    query = query.Where(x => x.FormerClass.EventEnd < toDateUpperBound);

                // if not null, synonymous class types is used instead of ClassTypeId
                if (clsFilter.SynonymousClassTypes != null)
                {
                    query = query.Where(x => clsFilter.SynonymousClassTypes.Contains(x.FormerClass.ClassTypeId));
                }
                else if (clsFilter.ClassTypeId.HasValue)
                {
                    query = query.Where(x => x.FormerClass.ClassTypeId == clsFilter.ClassTypeId);
                }

                if (!string.IsNullOrEmpty(clsFilter.Location))
                {
                    query = query.Where(x => x.FormerClass.Location.ToLower().Contains(clsFilter.Location.ToLower()));
                }

                if (clsFilter.ProfileId.HasValue)
                {
                    query = query.Where(x => x.FormerClass.ProfileId == clsFilter.ProfileId);
                }
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
