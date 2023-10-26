using AutomationSystem.Main.Contract.Persons.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Persons.AppLogic
{

    /// <summary>
    /// Service for persons administration
    /// </summary>
    public interface IPersonAdministration
    {

        // gets PersonList page model
        PersonListPageModel GetPersonListPageModel(PersonFilter filter, bool search);

        // gets person detail
        PersonDetail GetPersonDetail(long personId);

        // gets new person - input is profileId or ProfileConstants.WithoutProfileId constant
        PersonForEdit GetNewPersonForEdit(long profileIdOrWithout);

        // gets person for edit by person id
        PersonForEdit GetPersonForEdit(long personId);

        // gets persons for edit based on form
        PersonForEdit GetFormPersonForEdit(PersonForm form);


        // saves person
        long SavePerson(PersonForm person);

        // deletes person
        void DeletePerson(long personId);

    }

}
