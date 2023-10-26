using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Gui.Helpers;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic.MaterialRecipientIntegrations
{
    /// <summary>
    /// Materials recipient integration for class registrations
    /// </summary>
    public class RegistrationMaterialIntegration : IMaterialRecipientIntegration
    {
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IRegistrationStateProvider registrationStateProvider;
        private readonly IClassMaterialBusinessRules classMaterialBusinessRules;

        public RegistrationMaterialIntegration(
            IRegistrationDatabaseLayer registrationDb,
            IRegistrationStateProvider registrationStateProvider,
            IClassMaterialBusinessRules classMaterialBusinessRules)
        {
            this.registrationDb = registrationDb;
            this.registrationStateProvider = registrationStateProvider;
            this.classMaterialBusinessRules = classMaterialBusinessRules;
        }

        public EntityTypeEnum TypeId => EntityTypeEnum.MainClassRegistration;

        public List<RecipientId> GetAllRecipientIdsForClass(Class cls)
        {
            var registrations = GetClassRegistrationsByClassId(cls.ClassId);
            var result = registrations.Select(x => new RecipientId(EntityTypeEnum.MainClassRegistration, x.ClassRegistrationId)).ToList();

            return result;
        }

        public List<ClassMaterialMonitoringListItem> GetClassMaterialMonitoringListItems(
            Class cls,
            Func<RecipientId, ClassMaterialMonitoringListItem> monitoringListItemCreator)
        {
            var registrations = GetClassRegistrationsByClassId(cls.ClassId, ClassRegistrationIncludes.Addresses);
            var result = new List<ClassMaterialMonitoringListItem>();
            foreach (var registration in registrations)
            {
                var recipientId = new RecipientId(EntityTypeEnum.MainClassRegistration, registration.ClassRegistrationId);
                var listItem = monitoringListItemCreator(recipientId);

                var studentName = MainTextHelper.GetFullName(
                    AddressConvertor.ToLogicString(registration.StudentAddress.FirstName),
                    AddressConvertor.ToLogicString(registration.StudentAddress.LastName));
                var registrantName = registration.RegistrantAddress == null
                    ? null
                    : MainTextHelper.GetFullName(
                        AddressConvertor.ToLogicString(registration.RegistrantAddress.FirstName),
                        AddressConvertor.ToLogicString(registration.RegistrantAddress.LastName));
                listItem.Name = MainTextHelper.GetRegistrationName(studentName, registrantName);

                result.Add(listItem);
            }

            return result;
        }

        public MaterialAvailabilityResult ResolveRecipientRestrictions(long recipientId, Class cls)
        {
            var result = new MaterialAvailabilityResult();
            result.AreMaterialsAvailable = false;

            var registration = registrationDb.GetClassRegistrationById(recipientId);
            if (registration == null)
            {
                result.Message = $"There is no class registration with id {recipientId}";
                return result;
            }

            // checks whether registration type supports materials
            if (!classMaterialBusinessRules.AreMaterialsSupported(registration.RegistrationTypeId))
            {
                result.Message = $"Registration type {registration.RegistrationTypeId} doesn't support class materials";
                return result;
            }

            // checks registration state
            var registrationState = registrationStateProvider.GetRegistrationState(registration);
            if (registrationState != RegistrationState.Approved)
            {
                result.Message = $"Invalid registration state: {registrationState}";
                return result;
            }

            result.AreMaterialsAvailable = true;
            return result;
        }

        public long? CheckAndTryGetClassId(long recipientId, out string materialsDisabledMessage)
        {
            var registration = GetClassRegistrationById(recipientId);
            if (!classMaterialBusinessRules.AreMaterialsSupported(registration.RegistrationTypeId))
            {
                materialsDisabledMessage = "The registration does not support material distribution.";
                return null;
            }

            materialsDisabledMessage = null;
            return registration.ClassId;
        }

        #region private methods

        private List<ClassRegistration> GetClassRegistrationsByClassId(
            long classId,
            ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            var filter = new RegistrationFilter
            {
                ClassId = classId,
                RegistrationState = RegistrationState.Approved,
                RegistrationTypeIdsEnum = classMaterialBusinessRules.GetMaterialSupportingRegistrationTypes().ToList()
            };
            var result = registrationDb.GetRegistrationsByFilter(filter, includes);
            return result;
        }

        private ClassRegistration GetClassRegistrationById(
            long registrationId,
            ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no ClassRegistration with id {registrationId}.");
            }

            return result;
        }

        #endregion
    }
}
