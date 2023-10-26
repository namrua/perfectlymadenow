using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Persons.AppLogic.Convertors;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Core.Persons.System.Extensions;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Main.Model;
using CorabeuControl.Components;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using ProfileConstants = AutomationSystem.Main.Contract.Profiles.AppLogic.Models.ProfileConstants;

namespace AutomationSystem.Main.Core.Persons.AppLogic
{

    /// <summary>
    /// Service for persons administration
    /// </summary>
    public class PersonAdministration : IPersonAdministration
    {

        private readonly IPersonDatabaseLayer personDb;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IPersonConvertor personConvertor;
        private readonly IEventDispatcher eventDispatcher;
            
        // constructor
        public PersonAdministration(
            IPersonDatabaseLayer personDb,
            IProfileDatabaseLayer profileDb,
            IIdentityResolver identityResolver,
            IClassDatabaseLayer classDb,
            IPersonConvertor personConvertor,
            IEventDispatcher eventDispatcher)
        {
            this.personDb = personDb;
            this.profileDb = profileDb;
            this.identityResolver = identityResolver;
            this.classDb = classDb;
            this.personConvertor = personConvertor;
            this.eventDispatcher = eventDispatcher;
        }


        // gets PersonList page model
        public PersonListPageModel GetPersonListPageModel(PersonFilter filter, bool search)
        {
            var result = new PersonListPageModel(filter);
            result.WasSearched = search;

            // prepare profiles for read and insert (invariant: profiles for read are supperset of profiles for insert)
            var profileFilterForRead = identityResolver.GetGrantedProfilesForEntitle(Entitle.MainPersonsReadOnly);
            var profileFilterForInsert = identityResolver.GetGrantedProfilesForEntitle(Entitle.MainPersons);
            var profiles = profileDb.GetProfilesByFilter(profileFilterForRead);
            result.ProfilesForFilter = GetDropDownItemsFromProfiles(profiles, profileFilterForRead);
            result.ProfilesForInsert = GetDropDownItemsFromProfiles(profiles, profileFilterForInsert);

            // executes searching
            if (search)
            {
                var personFilter = personConvertor.MergeProfileFilterToPersonFilter(filter, profileFilterForRead);
                var persons = personDb.GetPersons(personFilter, PersonIncludes.Profile | PersonIncludes.Address);
                result.Items = persons.Select(personConvertor.ConvertToPersonListItem).ToList();

                // todo: #BICH batch item checking
            }
            return result;
        }


        // gets person detail
        public PersonDetail GetPersonDetail(long personId)
        {
            var person = GetPersonById(personId, PersonIncludes.AddressCountry | PersonIncludes.PersonRoles | PersonIncludes.Profile);
            identityResolver.CheckEntitleForPerson(Entitle.MainPersonsReadOnly, person);

            // converts to detail and sets access rights
            var result = personConvertor.ConvertToPersonDetail(person);
            result.CanEdit = result.CanDelete = identityResolver.IsEntitleGrantedForPerson(Entitle.MainPersons, person);
            result.CanDelete = result.CanDelete && CanDeletePerson(personId);
            return result;
        }


        // gets new person - input is profileId or ProfileConstants.WithoutProfileId constant
        public PersonForEdit GetNewPersonForEdit(long profileIdOrWithout)
        {
            var profileId = profileIdOrWithout == ProfileConstants.WithoutProfileId ? (long?) null : profileIdOrWithout;

            identityResolver.CheckEntitleForProfileId(Entitle.MainPersons, profileId);
            var result = personConvertor.InitializePersonForEdit();
            result.Form.ProfileId = profileId;
            return result;
        }

        // gets new persons for edit
        public PersonForEdit GetPersonForEdit(long personId)
        {
            // loads person and enums
            var person = GetPersonById(personId, PersonIncludes.Address | PersonIncludes.PersonRoles);
            identityResolver.CheckEntitleForPerson(Entitle.MainPersons, person);

            // creates result
            var result = personConvertor.InitializePersonForEdit();           
            result.Form = personConvertor.ConvertToPersonForm(person);                       
            return result;
        }


        // gets persons for edit based on form
        public PersonForEdit GetFormPersonForEdit(PersonForm form)
        {
            identityResolver.CheckEntitleForProfileId(Entitle.MainPersons, form.ProfileId);

            // creates result
            var result = personConvertor.InitializePersonForEdit();
            result.Form = form;
            return result;
        }


        // saves person
        public long SavePerson(PersonForm person)
        {
            var dbPerson = personConvertor.ConvertToPerson(person, true);           // there is no feature to disable person yet
            var result = person.PersonId;
            if (person.PersonId == 0)
            {
                identityResolver.CheckEntitleForPerson(Entitle.MainPersons, dbPerson);
                dbPerson.OwnerId = identityResolver.GetOwnerId();
                result = personDb.InsertPerson(dbPerson);
            }
            else
            {
                var toCheck = GetPersonById(person.PersonId);
                identityResolver.CheckEntitleForPerson(Entitle.MainPersons, toCheck);
                personDb.UpdatePerson(dbPerson);
            }

            return result;           
        }

        // deletes person
        public void DeletePerson(long personId)
        {
            var toCheck = GetPersonById(personId);
            identityResolver.CheckEntitleForPerson(Entitle.MainPersons, toCheck);

            if (!CanDeletePerson(personId))
                throw new InvalidOperationException($"Person with id {personId} was assigned to some class or preferences and cannot be deleted.");
            personDb.DeletePerson(personId);            
        }


        #region private methods

        // gets person by id from database
        private Person GetPersonById(long personId, PersonIncludes includes = PersonIncludes.None)
        {
            var person = personDb.GetPersonById(personId, includes);
            if (person == null)
                throw new ArgumentException($"There is no Person with id {personId}.");
            return person;
        }


        // determines whether person id can be deleted
        private bool CanDeletePerson(long personId)
        {
            var result = eventDispatcher.Check(new PersonDeletingEvent(personId));
            return result;
        }
        

        // converts list of profiles to DropDownItem
        private List<DropDownItem> GetDropDownItemsFromProfiles(List<Model.Profile> profiles, ProfileFilter filter = null)
        {
            var result = new List<DropDownItem>();

            // process additional instructions by filter
            if (filter != null)
            {
                // adds default profile
                if (filter.IncludeDefaultProfile)
                    result.Add(DropDownItem.Item(ProfileConstants.WithoutProfileId, "Without profile"));

                // executes additional filtering
                if (filter.ProfileIds != null)
                    profiles = profiles.Where(x => filter.ProfileIds.Contains(x.ProfileId)).ToList();
            }

            // converts profiles to DropDowItems
            result.AddRange(profiles.Select(x => DropDownItem.Item(x.ProfileId, x.Name)));
            return result;
        }


        #endregion

    }

}
