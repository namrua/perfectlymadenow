using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Persons.AppLogic
{
    /// <summary>
    /// Helps to access person or its name quickly
    /// </summary>
    public interface IPersonIdMapper
    {

        // gets person name by id
        string GetPersonNameById(long? personId);

        // gets person by id - if not exists, returns null
        Person TryGetPersonById(long? personId);

        // gets person by id - if not exists, throws exception
        Person GetPersonById(long personId);

    }

}
