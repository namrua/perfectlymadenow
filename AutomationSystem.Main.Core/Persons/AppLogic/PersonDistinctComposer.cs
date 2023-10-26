using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Persons.AppLogic
{

    /// <summary>
    /// Allows to compose sets of persons (useful for unions of different role types)
    /// Ensures distinction of the sets
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PersonDistinctComposer<T> : IPersonDistinctComposer<T> where T : class
    {

        private readonly HashSet<long> usedIds = new HashSet<long>();
        private readonly List<T> set = new List<T>();
        private readonly Func<long, T> creator;


        // constructor
        public PersonDistinctComposer(Func<long, T> creator)
        {
            this.creator = creator;
        }


        // adds one person into the set - returns immediate result of inner conversion, 
        // if pesonId is null, returns null
        public T AddPerson(long? personId)
        {
            if (!personId.HasValue || usedIds.Contains(personId.Value))
                return null;

            var result = creator(personId.Value);
            set.Add(result);
            usedIds.Add(personId.Value);
            return result;
        }

        // adds list of persons into the set - returns immediate result of inner conversion,        
        public List<T> AddPersons(IEnumerable<long> personsIds)
        {
            var unusedIds = personsIds.Where(x => !usedIds.Contains(x)).Distinct().ToList();
            var result = unusedIds.Select(x => creator(x)).ToList();
            set.AddRange(result);
            usedIds.UnionWith(unusedIds);
            return result;
        }


        // note: can be created as Extension method
        // filters person by role, returns immediate result of inner conversion
        public List<T> AddClassPersonsWithRole(IEnumerable<ClassPerson> classPersons, PersonRoleTypeEnum role)
        {
            var personIds = classPersons.Where(x => role == x.RoleTypeId).Select(x => x.PersonId).ToList();
            return AddPersons(personIds);
        }

        public List<T> AddDistanceClassTemplatePersonWithRoles(IEnumerable<DistanceClassTemplatePerson> templatePersons, PersonRoleTypeEnum role)
        {
            var personIds = templatePersons.Where(x => role == x.RoleTypeId).Select(x => x.PersonId).ToList();
            return AddPersons(personIds);
        }


        // pops collected set of converted objects
        // object can continue with creating another set, distinct with all previous
        public List<T> Pop()
        {
            var result = set.ToList();
            set.Clear();
            return result;
        }

    }

}
