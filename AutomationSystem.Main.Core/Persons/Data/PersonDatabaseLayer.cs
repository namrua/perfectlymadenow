using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic.Models;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Classes.Data.Extensions;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Persons.Data.Extensions;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Main.Core.Persons.Data
{

    /// <summary>
    /// Provides Person and Address database layer
    /// </summary>
    public class PersonDatabaseLayer : IPersonDatabaseLayer
    {

        // private components
        private readonly IAddressConvertor addressConvertor;


        // constructor
        public PersonDatabaseLayer(IAddressConvertor addressConvertor)
        {
            this.addressConvertor = addressConvertor;
        }

        // get role types
        public List<PersonRoleType> GetPersonRoleTypes()
        {
            using (var context = new MainEntities())
            {
                var result = context.PersonRoleTypes.ToList();
                return result;
            }
        }


        // gets list of persons       
        public List<Person> GetPersons(PersonFilter filter = null, PersonIncludes includes = PersonIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.Persons.AddIncludes(includes).Filter(filter).ToList();
                result = result.Select(x => PersonRemoveInactive.RemoveInactiveForPerson(x, includes)).ToList();
                return result;
            }
        }


        // gets list of persons by ids       
        public List<Person> GetPersonsByIds(IEnumerable<long> personIds, PersonIncludes includes = PersonIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.Persons.AddIncludes(includes).Active().Where(x => personIds.Contains(x.PersonId)).ToList();
                result = result.Select(x => PersonRemoveInactive.RemoveInactiveForPerson(x, includes)).ToList();
                return result;
            }
        }

        // gets person by id        
        public Person GetPersonById(long personId, PersonIncludes includes = PersonIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.Persons.AddIncludes(includes).Active().FirstOrDefault(x => x.PersonId == personId);
                result = PersonRemoveInactive.RemoveInactiveForPerson(result, includes);                
                return result;
            }
        }


        // gets minimized list of persons for profileId - - null returns only shared profiles 
        public List<PersonMinimized> GetMinimizedPersonsByProfileId(long? profileId)
        {
            using (var context = new MainEntities())
            {
                var query = context.Persons.Active().Where(x => !x.ProfileId.HasValue || x.ProfileId == profileId);
                var result = QueryMinimizedPerons(query);
                
                return result;
            }
        }


        // gets minimized list of persons by ids
        public List<PersonMinimized> GetMinimizedPersonsByIds(IEnumerable<long> personIds)
        {
            using (var context = new MainEntities())
            {
                var query = context.Persons.Active().Where(x => personIds.Contains(x.PersonId));
                var result = QueryMinimizedPerons(query);
                return result;
            }
        }

        

        // gets all used distance coordinators - !!! AssignedRoles and DefaultRoles are ignored !!!
        public List<PersonMinimized> GetUsedDistanceCoordinators(List<long> profileIds)
        {
            using (var context = new MainEntities())
            {
                // defines filter
                var classFilter = new ClassFilter
                {
                    ClassCategoryId = ClassCategoryEnum.DistanceClass,
                    OpenAndCompleted = true,
                    ProfileIds = profileIds
                };

                // gets all distance coordinators
                var persons = context.Classes.Filter(classFilter).Select(x => x.Coordinator).Distinct()
                    .Select(x => new
                    {
                        x.PersonId,
                        x.Address.FirstName,
                        x.Address.LastName,
                        x.ProfileId
                    }).ToList();

                // converts them to PersonMinimized without roles
                var result = persons.Select(x => new PersonMinimized
                {
                    PersonId = x.PersonId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Name = MainTextHelper.GetFullName(x.FirstName, x.LastName)
                }).ToList();
                return result;
            }
        }

        // checks if person is link to profile
        public bool AnyPersonOnProfile(long profileId)
        {
            using (var context = new MainEntities())
            {
                var result = context.Persons.Active().Any(x => x.ProfileId == profileId);
                return result;
            }
        }


        // insert person
        public long InsertPerson(Person person)
        {
            using (var context = new MainEntities())
            {
                context.Persons.Add(person);
                context.SaveChanges();
                return person.PersonId;
            }
        }

        // update person
        public void UpdatePerson(Person person)
        {
            using (var context = new MainEntities())
            {
                // updates person
                var toUpdate = context.Persons.Include("Address").Active().FirstOrDefault(x => x.PersonId == person.PersonId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Person with id {person.PersonId}.");

                toUpdate.Email = person.Email;
                toUpdate.Phone = person.Phone;
                toUpdate.AssignedUserId = person.AssignedUserId;
                toUpdate.CoordinatorNumber = person.CoordinatorNumber;
                toUpdate.Active = person.Active;
                addressConvertor.UpdateAddresss(toUpdate.Address, person.Address);

                // updates roles
                var personRoles = context.PersonRoles.Active().Where(x => x.PersonId == person.PersonId);
                var updateResolver = new SetUpdateResolver<PersonRole, PersonRoleTypeEnum>(
                    x => x.RoleTypeId, (origItem, newItem) => { origItem.IsDefault = newItem.IsDefault; });
                var strategy = updateResolver.ResolveStrategy(personRoles, person.PersonRoles);
                context.PersonRoles.AddRange(strategy.ToAdd);
                context.PersonRoles.RemoveRange(strategy.ToDelete);

                // saves data
                context.SaveChanges();
            }
        }

        // delete person
        public void DeletePerson(long personId)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.Persons.Include("Address").Active().FirstOrDefault(x => x.PersonId == personId);
                if (toDelete == null) return;
                    
                context.Addresses.Remove(toDelete.Address);
                context.Persons.Remove(toDelete);
                context.SaveChanges();
            }
        }


        #region private methods

        // query minimezed persons
        private List<PersonMinimized> QueryMinimizedPerons(IQueryable<Person> query)
        {
            // queries persons
            var persons = query.Select(x => new
            {
                x.PersonId,
                x.Address.FirstName,
                x.Address.LastName,
                PersonRoles = x.PersonRoles.Where(y => !y.Deleted)
            }).ToList();

            // converts to minimized persons
            var result = persons.Select(x => new PersonMinimized
            {
                PersonId = x.PersonId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Name = MainTextHelper.GetFullName(x.FirstName, x.LastName),
                AssignedRoles = new HashSet<PersonRoleTypeEnum>(x.PersonRoles.Select(y => y.RoleTypeId).ToList()),
                DefaultRoles =
                    new HashSet<PersonRoleTypeEnum>(x.PersonRoles.Where(y => y.IsDefault).Select(y => y.RoleTypeId).ToList())
            }).ToList();
            return result;
        }

        #endregion

    }

}
