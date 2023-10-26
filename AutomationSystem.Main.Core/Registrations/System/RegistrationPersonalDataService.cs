using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Handlers;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Model;
using System;

namespace AutomationSystem.Main.Core.Registrations.System
{
    public class RegistrationPersonalDataService : IRegistrationPersonalDataService
    {
        private readonly IClassMaterialDistributionHandler materialDistributionHandler;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IRegistrationLogicProvider registrationLogicProvider;
        private readonly IMainAsyncRequestManager requestManager;

        public RegistrationPersonalDataService(
            IClassMaterialDistributionHandler materialDistributionHandler,
            IRegistrationDatabaseLayer registrationDb,
            IRegistrationLogicProvider registrationLogicProvider,
            IMainAsyncRequestManager requestManager)
        {
            this.materialDistributionHandler = materialDistributionHandler;
            this.registrationDb = registrationDb;
            this.registrationLogicProvider = registrationLogicProvider;
            this.requestManager = requestManager;
        }

        public long SaveRegistration(BaseRegistrationForm form)
        {
            var isNewRegistration = form.ClassRegistrationId == 0;
            var oldRegistration = isNewRegistration ? null : GetRegistrationById(form.ClassRegistrationId);
            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(form.RegistrationTypeId);
            var result = service.SaveRegistration(form, false, ApprovementTypeEnum.ManualApprovement);

            if (isNewRegistration) return result;
            var registration = GetRegistrationById(form.ClassRegistrationId);
            if (!registration.IsApproved || registration.IsCanceled)
            {
                return result;
            }

            requestManager.AddIntegrationRequestForClassRegistration(registration, AsyncRequestTypeEnum.AddToOuterSystem, (int)SeverityEnum.High);

            materialDistributionHandler.HandleRegistrationChange(oldRegistration, registration);
            return result;
        }

        #region private methods

        private ClassRegistration GetRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class registration with id {registrationId}.");
            }

            return result;
        }

        #endregion
    }
}
