using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Persons.AppLogic
{


    /// <summary>
    /// Allows to compose sets of persons (useful for unions of different role types)
    /// Ensures distinction of the sets
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPersonDistinctComposer<T> where T : class
    {

        // adds one person into the set - returns immediate result of inner conversion, 
        // if pesonId is null, returns null
        T AddPerson(long? personId);

        // adds list of persons into the set - returns immediate result of inner conversion,        
        List<T> AddPersons(IEnumerable<long> personsIds);

        // note: can be created as Extension method
        // filters person by role, returns immediate result of inner conversion
        List<T> AddClassPersonsWithRole(IEnumerable<ClassPerson> classPersons, PersonRoleTypeEnum role);

        List<T> AddDistanceClassTemplatePersonWithRoles(IEnumerable<DistanceClassTemplatePerson> templatePersons, PersonRoleTypeEnum role);

        // pops collected set of converted objects
        // object can continue with creating another set, distinct with all previous
        List<T> Pop();

    }

}
