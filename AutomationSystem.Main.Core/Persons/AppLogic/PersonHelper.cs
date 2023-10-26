using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Core.Persons.AppLogic
{

    /// <summary>
    /// Person helper provides functionality for filling view component with persons
    /// </summary>
    public class PersonHelper : IPersonHelper
    {

        // private fields
        public IEnumerable<PersonMinimized> persons;
        public Dictionary<long, PersonMinimized> personMap;

        // constructor - minimized persons
        public PersonHelper(IEnumerable<PersonMinimized> persons = null)
        {
            this.persons = persons ?? new List<PersonMinimized>();
            personMap = this.persons.ToDictionary(x => x.PersonId);
        }       


        // gets person name by id
        public string GetPersonNameById(long? personId)
        {
            if (!personId.HasValue)
                return null;
            var result = personMap.TryGetValue(personId.Value, out var personResult) ? personResult.Name : null;
            return result;
        }

        // get default ids
        public long? GetDefaultPersonId(PersonRoleTypeEnum role)
        {
            var defaultPerson = persons.FirstOrDefault(x => x.AssignedRoles.Contains(role) && x.DefaultRoles.Contains(role));
            return defaultPerson?.PersonId;
        }               

        // gets default ids for role
        public List<long> GetDefaultPersonIds(PersonRoleTypeEnum role)
        {
            var result = persons.Where(x => x.AssignedRoles.Contains(role) && x.DefaultRoles.Contains(role)).Select(x => x.PersonId).ToList();
            return result;
        }

        // gets picker items for given role
        public List<PickerItem> GetPickerItemsForRole(PersonRoleTypeEnum role)
        {
            var result = persons.Where(x => x.AssignedRoles.Contains(role)).Select(person => PickerItem.Item(person.PersonId, person.Name)).ToList();
            return result;
        }

    }

}
