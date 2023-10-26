using System.Collections.Generic;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Persons.Data
{
    /// <summary>
    /// Provides Person and Address database layer
    /// </summary>
    public interface IPersonDatabaseLayer
    {

        // get role types
        List<PersonRoleType> GetPersonRoleTypes();


        // gets list of persons
        List<Person> GetPersons(PersonFilter filter = null, PersonIncludes includes = PersonIncludes.None);

        // gets list of persons by ids
        List<Person> GetPersonsByIds(IEnumerable<long> personIds, PersonIncludes includes = PersonIncludes.None);

        // gets person by id
        Person GetPersonById(long personId, PersonIncludes includes = PersonIncludes.None);


        // gets minimized list of persons for profileId - null returns only shared profiles
        List<PersonMinimized> GetMinimizedPersonsByProfileId(long? profileId);

        // gets minimized list of persons by ids
        List<PersonMinimized> GetMinimizedPersonsByIds(IEnumerable<long> personIds);

        // gets all used distance coordinators - !!! AssignedRoles and DefaultRoles are ignored !!!
        List<PersonMinimized> GetUsedDistanceCoordinators(List<long> profileIds);

        // checks if person is link to profile
        bool AnyPersonOnProfile(long profileId);
        
        // insert person
        long InsertPerson(Person person);

        // update person
        void UpdatePerson(Person person);

        // delete person
        void DeletePerson(long personId);

        
    }

}
