using System.Data.Entity.Infrastructure;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Persons.Data.Extensions
{
    public static class PersonIncludeExtensions
    {
        // adds includes for Person
        public static DbQuery<Person> AddIncludes(this DbQuery<Person> query, PersonIncludes includes)
        {
            if (includes.HasFlag(PersonIncludes.Address))
                query = query.Include("Address");
            if (includes.HasFlag(PersonIncludes.AddressCountry))
                query = query.Include("Address.Country");
            if (includes.HasFlag(PersonIncludes.PersonRoles))
                query = query.Include("PersonRoles");
            if (includes.HasFlag(PersonIncludes.Profile))
                query = query.Include("Profile");
            return query;
        }
    }
}
