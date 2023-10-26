using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.MaterialRecipientIntegrations
{
    /// <summary>
    /// Materials recipient integration for class registrations
    /// </summary>
    public class PersonMaterialIntegration : IMaterialRecipientIntegration
    {
        private readonly IPersonDatabaseLayer personDb;
        private readonly IClassMaterialBusinessRules classMaterialBusinessRules;

        public PersonMaterialIntegration(IPersonDatabaseLayer personDb, IClassMaterialBusinessRules classMaterialBusinessRules)
        {
            this.personDb = personDb;
            this.classMaterialBusinessRules = classMaterialBusinessRules;
        }

        public EntityTypeEnum TypeId => EntityTypeEnum.MainPerson;

        public List<RecipientId> GetAllRecipientIdsForClass(Class cls)
        {
            if (cls.ClassPersons == null)
            {
                throw new InvalidOperationException("ClassPersons is not included into Class object.");
            }

            var personIds = GetPersonIds(cls);
            var result = personIds.Select(x => new RecipientId(EntityTypeEnum.MainPerson, x)).ToList();

            return result;
        }

        public List<ClassMaterialMonitoringListItem> GetClassMaterialMonitoringListItems(
            Class cls,
            Func<RecipientId, ClassMaterialMonitoringListItem> monitoringListItemCreator)
        {
            var personIds = GetPersonIds(cls);
            var persons = personDb.GetMinimizedPersonsByIds(personIds);

            var result = new List<ClassMaterialMonitoringListItem>();
            foreach (var person in persons)
            {
                var recipientId = new RecipientId(EntityTypeEnum.MainPerson, person.PersonId);
                var listItem = monitoringListItemCreator(recipientId);

                listItem.Name = person.Name;

                result.Add(listItem);
            }

            return result;
        }

        public MaterialAvailabilityResult ResolveRecipientRestrictions(long recipientId, Class cls)
        {
            var result = new MaterialAvailabilityResult();
            result.AreMaterialsAvailable = false;

            // checks whether personId is assigned to class and have material supporting role
            var personRoles = classMaterialBusinessRules.GetMaterialSupportingPersonRoles();
            var personIds = ClassConvertor.GetClassPersonIdsForRoles(cls, personRoles);
            if (!personIds.Contains(recipientId))
            {
                result.Message = "Person is not assigned to class or does not have material supporting role";
                return result;
            }

            result.AreMaterialsAvailable = true;
            return result;
        }

        public long? CheckAndTryGetClassId(long recipientId, out string materialsDisabledMessage)
        {
            materialsDisabledMessage = "Person's materials administration cannot be accessed only by recipientId";
            return null;
        }

        #region private methods

        private HashSet<long> GetPersonIds(Class cls)
        {
            var personRoles = classMaterialBusinessRules.GetMaterialSupportingPersonRoles();
            var personIds = ClassConvertor.GetClassPersonIdsForRoles(cls, personRoles);
            return personIds;
        }

        #endregion
    }
}
