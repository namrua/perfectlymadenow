using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Enums.System;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;

namespace AutomationSystem.Main.Core.Persons.AppLogic.Convertors
{

    /// <summary>
    /// Converts persons entities
    /// </summary>
    public class PersonConvertor : IPersonConvertor
    {

        private readonly IEnumDatabaseLayer enumDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IAddressConvertor addressConvertor;

        private readonly Lazy<Dictionary<PersonRoleTypeEnum, PersonRoleType>> roleMap;

        // construcor
        public PersonConvertor(
            IEnumDatabaseLayer enumDb,
            IPersonDatabaseLayer personDb,
            IAddressConvertor addressConvertor)
        {
            this.enumDb = enumDb;
            this.personDb = personDb;
            this.addressConvertor = addressConvertor;

            roleMap = new Lazy<Dictionary<PersonRoleTypeEnum, PersonRoleType>>(() => personDb.GetPersonRoleTypes().ToDictionary(x => x.PersonRoleTypeId, y => y));
        }



        // merges person filter and profile filter
        public PersonFilter MergeProfileFilterToPersonFilter(PersonFilter personFilter, ProfileFilter profileFilter)
        {
            personFilter = personFilter ?? new PersonFilter();
            personFilter.ProfileIds = profileFilter.ProfileIds;
            personFilter.IncludeDefaultProfile = profileFilter.IncludeDefaultProfile;
            return personFilter;
        }


        // initializes PersonForEdit
        public PersonForEdit InitializePersonForEdit()
        {
            var result = new PersonForEdit();
            result.Countries = enumDb.GetItemsByFilter(EnumTypeEnum.Country).SortDefaultCountryFirst();
            result.RoleTypes = personDb.GetPersonRoleTypes();
            return result;
        }


        // converts db Person to PersonListItem
        public PersonListItem ConvertToPersonListItem(Person person)
        {
            if (person.Address == null)
                throw new InvalidOperationException("Address is not included into Person object.");
            if (person.ProfileId.HasValue && person.Profile == null)
                throw new InvalidOperationException("Profile is not included into Person object.");

            var result = new PersonListItem
            {
                PersonId = person.PersonId,
                FullName = MainTextHelper.GetFullName(person.Address.FirstName, person.Address.LastName),
                Email = person.Email,
                Phone = person.Phone,
                Profile = person.Profile?.Name
            };
            return result;
        }


        // converts db Person to PersonForm
        public PersonForm ConvertToPersonForm(Person person)
        {
            if (person.Address == null)
                throw new InvalidOperationException("Address is not included into Person object.");
            if (person.PersonRoles == null)
                throw new InvalidOperationException("PersonRoles is not included into Person object.");

            // creates model form
            var result = new PersonForm
            {
                PersonId = person.PersonId,
                ProfileId = person.ProfileId,
                Email = person.Email,
                Phone = person.Phone,
                CoordinatorNumber = person.CoordinatorNumber,
                AssignedUserId = person.AssignedUserId,
                Address = addressConvertor.ConvertToAddressForm(person.Address),
                PersonRoles = person.PersonRoles.Select(x => x.RoleTypeId).ToArray(),
                DefaultPersonRoles = person.PersonRoles.Where(x => x.IsDefault).Select(x => x.RoleTypeId).ToArray()
            };
            return result;
        }

        // converts PersonForm to db Person
        public Person ConvertToPerson(PersonForm form, bool active)
        {
            var result = new Person();
            result.PersonId = form.PersonId;
            result.ProfileId = form.ProfileId;
            result.Email = form.Email;
            result.Phone = form.Phone;
            result.CoordinatorNumber = form.CoordinatorNumber;
            result.AssignedUserId = form.AssignedUserId;
            result.Active = active;                                   
            result.Address = addressConvertor.ConvertToAddress(form.Address, false);
            var defaultPersonRoles = new HashSet<PersonRoleTypeEnum>(form.DefaultPersonRoles);
            foreach (var roleType in form.PersonRoles)
            {
                var personRole = new PersonRole();
                personRole.PersonId = form.PersonId;
                personRole.RoleTypeId = roleType;
                personRole.IsDefault = defaultPersonRoles.Contains(roleType);
                result.PersonRoles.Add(personRole);
            }
            return result;
        }


        // converts Person to PersonDetail
        public PersonDetail ConvertToPersonDetail(Person person)
        {
            if (person.Address == null)
                throw new InvalidOperationException("Address is not included into Person object.");
            if (person.ProfileId.HasValue && person.Profile == null)
                throw new InvalidOperationException("Profile is not included into Person object.");
            if (person.PersonRoles == null)
                throw new InvalidOperationException("PersonRoles is not included into Person object.");

            var result = new PersonDetail
            {
                PersonId = person.PersonId,
                ProfileId = person.ProfileId,
                Profile = person.Profile?.Name,
                Address = addressConvertor.ConvertToAddressDetail(person.Address),
                Email = person.Email,
                Phone = person.Phone,
                CoordinatorNumber = person.CoordinatorNumber,
                AssignedRoles = person.PersonRoles.Select(ConvertToPersonRoleDetail).ToList()
            };
            return result;
        }


        // converts Person to PersonDetail
        public PersonShortDetail ConvertToPersonShortDetail(Person person)
        {            
            if (person.PersonRoles == null)
                throw new InvalidOperationException("PersonRoles is not included into Person object.");

            var result = ConvertToPersonShortDetailWithCustomRoles(person, person.PersonRoles.Select(x => x.RoleTypeId));           
            return result;
        }

        // converts Person to PersonDetail with custom roles
        public PersonShortDetail ConvertToPersonShortDetailWithCustomRoles(Person person, IEnumerable<PersonRoleTypeEnum> customRoles)
        {
            if (person.Address == null)
                throw new InvalidOperationException("Address is not included into Person object.");

            var result = new PersonShortDetail
            {
                PersonId = person.PersonId,
                FirstName = person.Address.FirstName,
                LastName = person.Address.LastName,
                Email = person.Email,
                Roles = customRoles.Select(x => roleMap.Value[x].Description).ToList()
            };
            result.Name = MainTextHelper.GetFullName(result.FirstName, result.LastName);
            
            return result;
        }



        #region roles convertors

        // converts PersonRole t PersonRoleDetail
        public PersonRoleDetail ConvertToPersonRoleDetail(PersonRole personRole)
        {
            var result = new PersonRoleDetail
            {
                RoleTypeId = personRole.RoleTypeId,
                RoleType = roleMap.Value[personRole.RoleTypeId].Description,
                IsDefault = personRole.IsDefault
            };
            return result;
        }

        #endregion

    }
    
}
