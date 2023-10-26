using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Linq;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;

namespace AutomationSystem.Main.Core.Persons.Data.Extensions
{
    public static class PersonFilterExtensions
    {
        // selects Person entities by filter, includes restriction on Active items
        public static IQueryable<Person> Filter(this IQueryable<Person> query, PersonFilter filter)
        {
            query = query.Active();
            if (filter == null)
                return query;

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => (x.Address.FirstName + " " + x.Address.LastName).ToLower().Contains(filter.Name.ToLower()));
            if (!string.IsNullOrEmpty(filter.Contact))
                query = query.Where(x => x.Email.ToLower().Contains(filter.Contact.ToLower())
                                         || x.Phone.ToLower().Contains(filter.Contact.ToLower()));
            if (filter.ProfileId.HasValue)
            {
                query = filter.ProfileId != ProfileConstants.WithoutProfileId
                    ? query.Where(x => x.ProfileId == filter.ProfileId)
                    : query.Where(x => !x.ProfileId.HasValue);
            }

            // identity filtering by profileId
            if (filter.ProfileIds != null)
            {
                var profileIdsForNullable = filter.ProfileIds.Cast<long?>().ToList();
                if (filter.IncludeDefaultProfile)
                    profileIdsForNullable.Add(null);
                query = query.Where(x => profileIdsForNullable.Contains(x.ProfileId));
            }
            if (!filter.IncludeDefaultProfile)
                query = query.Where(x => !x.ProfileId.HasValue);

            return query;
        }
    }
}
