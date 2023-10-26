using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.Components;
using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.Persons.AppLogic
{
    /// <summary>
    /// Person helper provides functionality for filling view component with persons
    /// </summary>
    public interface IPersonHelper
    {

        // gets person name by id
        string GetPersonNameById(long? personId);

        // get default ids
        long? GetDefaultPersonId(PersonRoleTypeEnum role);

        // gets default ids for role
        List<long> GetDefaultPersonIds(PersonRoleTypeEnum role);

        // gets picker items for given role
        List<PickerItem> GetPickerItemsForRole(PersonRoleTypeEnum role);

    }
}
