using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Integration;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;
using System;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{

    /// <summary>
    /// Registration administration service
    /// </summary>
    public class RegistrationIntegrationAdministration : IRegistrationIntegrationAdministration
    {
        private readonly IIdentityResolver identityResolver;
        private readonly IIntegrationAdministration integrationAdministration;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IMainAsyncRequestManager requestManager;
        private readonly IWebExRegistrationConvertor webExRegistrationConvertor;
        private readonly IRegistrationStateProvider registrationStateProvider;
        private readonly IRegistrationTypeResolver registrationTypeResolver;


        // constructor
        public RegistrationIntegrationAdministration(
            IIdentityResolver identityResolver,
            IIntegrationAdministration integrationAdministration,
            IRegistrationDatabaseLayer registrationDb,
            IMainAsyncRequestManager requestManager,
            IWebExRegistrationConvertor webExRegistrationConvertor,
            IRegistrationStateProvider registrationStateProvider,
            IRegistrationTypeResolver registrationTypeResolver)
        {
            this.identityResolver = identityResolver;
            this.integrationAdministration = integrationAdministration;
            this.registrationDb = registrationDb;
            this.requestManager = requestManager;
            this.webExRegistrationConvertor = webExRegistrationConvertor;
            this.registrationStateProvider = registrationStateProvider;
            this.registrationTypeResolver = registrationTypeResolver;
        }
        
        public RegistrationIntegrationPageModel GetRegistrationIntegrationPageModel(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class | ClassRegistrationIncludes.AddressesCountry);
            identityResolver.CheckEntitleForClass(registration.Class);

            var result = new RegistrationIntegrationPageModel
            {
                ClassId = registration.ClassId,
                ClassRegistrationId = registration.ClassRegistrationId,
                RegistrationState = registrationStateProvider.GetRegistrationState(registration)

            };
            
            if (registrationTypeResolver.IsWwaRegistration(registration.RegistrationTypeId))
            {
                result.IsIntegrationDisabled = true;
                result.IntegrationDisabledMessage = "This registration does not support integration.";
                return result;
            }
            
            if (!registrationStateProvider.WasIntegrated(registration))
            {
                result.IsIntegrationDisabled = true;
                result.IntegrationDisabledMessage = "This registration has not approved and integrated.";
                return result;
            }
            
            if (registration.Class.IntegrationTypeId == IntegrationTypeEnum.NoIntegration)
            {
                result.IsIntegrationDisabled = true;
                result.IntegrationDisabledMessage = "Registration's class has no integration.";
                return result;
            }
      
            result.Attendees = ComputeWebExStateSummary(registration);
            return result;
        }
        
        public List<IntegrationStateSummary> ExecuteIntegrationRequest(long registrationId, AsyncRequestTypeEnum integrationRequestType)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class | ClassRegistrationIncludes.AddressesCountry);
            identityResolver.CheckEntitleForClass(registration.Class);
            
            if (registrationTypeResolver.IsWwaRegistration(registration.RegistrationTypeId))
                throw new InvalidOperationException($"Class registration with id {registrationId} is WWA and does not support integration with WebEx.");

            if (registration.Class.IntegrationTypeId != IntegrationTypeEnum.WebExProgram)
                throw new InvalidOperationException(
                    $"Class registration with id {registrationId} cannot by integrated with WebEx due to invalid integration state {registration.Class.IntegrationTypeId}");
            
            requestManager.AddIntegrationRequestForClassRegistration(registration, integrationRequestType, (int)SeverityEnum.Fatal);
            
            var result = ComputeWebExStateSummary(registration);
            return result;
        }
        
        #region private methods
        private ClassRegistration GetRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
                throw new ArgumentException($"There is no Class registration with id {registrationId}.");
            return result;
        }
        
        private List<IntegrationStateSummary> ComputeWebExStateSummary(ClassRegistration registration)
        {
            var systemState = webExRegistrationConvertor.ConvertToIntegrationState(registration);
            var result = integrationAdministration.GetIntegrationStateSummary(systemState, EntityTypeEnum.MainClassRegistration,
                registration.ClassRegistrationId, registration.Class.IntegrationEntityId ?? 0);
            return result;
        }
        #endregion

    }

}
