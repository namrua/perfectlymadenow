using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Linq;

namespace AutomationSystem.Main.Core.Persons.Data.Extensions
{
    public static class PersonRemoveInactive
    {
        // removes inactive includes for Person
        public static Person RemoveInactiveForPerson(Person entity, PersonIncludes includes)
        {
            if (entity == null)
                return null;
            if (includes.HasFlag(PersonIncludes.PersonRoles))
                entity.PersonRoles = entity.PersonRoles.AsQueryable().Active().ToList();
            return entity;
        }
    }
}
