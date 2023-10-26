using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Persons.AppLogic.Convertors
{


    /// <summary>
    /// Converts persons entities
    /// </summary>
    public interface IPersonConvertor
    {

        // merges person filter and profile filter
        PersonFilter MergeProfileFilterToPersonFilter(PersonFilter personFilter, ProfileFilter profileFilter);


        // initializes PersonForEdit
        PersonForEdit InitializePersonForEdit();

        // converts db Person to PersonListItem
        PersonListItem ConvertToPersonListItem(Person person);

        // converts db Person to PersonForm
        PersonForm ConvertToPersonForm(Person person);

        // converts PersonForm to db Person
        Person ConvertToPerson(PersonForm form, bool active);


        // converts Person to PersonDetail
        PersonDetail ConvertToPersonDetail(Person person);


        // converts Person to PersonDetail
        PersonShortDetail ConvertToPersonShortDetail(Person person);

        // converts Person to PersonDetail with custom roles
        PersonShortDetail ConvertToPersonShortDetailWithCustomRoles(Person person, IEnumerable<PersonRoleTypeEnum> customRoles);

    }

}
