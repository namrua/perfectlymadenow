using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Persons.AppLogic
{

    /// <summary>
    /// Helps to access person or its name quickly
    /// </summary>
    public class PersonIdMapper : IPersonIdMapper
    {

        private readonly Dictionary<long, Person> map;

        // constructor
        public PersonIdMapper(IEnumerable<Person> persons = null)
        {
            map = persons?.ToDictionary(x => x.PersonId, y => y) ?? new Dictionary<long, Person>();
        }
        

        // gets person name by id
        public string GetPersonNameById(long? personId)
        {
            var person = TryGetPersonById(personId);
            if (person == null)
                return null;
            var result = MainTextHelper.GetFullName(person.Address.FirstName, person.Address.LastName);
            return result;
        }

        // gets person by id - if not exists, returns null
        public Person TryGetPersonById(long? personId)
        {
            if (!personId.HasValue || !map.TryGetValue(personId.Value, out var result))
                return null;
            return result;
        }

        // gets person by id with checking
        public Person GetPersonById(long personId)
        {
            var result = TryGetPersonById(personId);
            if (result == null)
                throw new ArgumentException($"There is no Person with id {personId}.");
            return result;
        }       

    }

}
