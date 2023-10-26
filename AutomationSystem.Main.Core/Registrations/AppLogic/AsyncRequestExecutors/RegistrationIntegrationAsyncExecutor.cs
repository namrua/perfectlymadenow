using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Integration.System;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.AsyncRequestExecutors
{
    /// <summary>
    /// Executes integration of registrations
    /// </summary>
    public class RegistrationIntegrationAsyncExecutor : IAsyncRequestExecutor
    {
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IRegistrationStateProvider registrationStateProvider;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        private readonly ITracerFactory tracerFactory;
        private readonly Dictionary<IntegrationTypeEnum, IRegistrationIntegrationRequestHandlerFactory> handlerFactories;

        private ITracer tracer;

        public bool CanReportIncident => true;

        public RegistrationIntegrationAsyncExecutor(
            IRegistrationDatabaseLayer registrationDb,
            IEnumerable<IRegistrationIntegrationRequestHandlerFactory> requestHandlerFactories,
            ITracerFactory tracerFactory,
            IRegistrationStateProvider registrationStateProvider,
            IRegistrationTypeResolver registrationTypeResolver)
        {            
            this.registrationDb = registrationDb;
            this.tracerFactory = tracerFactory;
            this.registrationStateProvider = registrationStateProvider;
            this.registrationTypeResolver = registrationTypeResolver;

            handlerFactories = new Dictionary<IntegrationTypeEnum, IRegistrationIntegrationRequestHandlerFactory>();
            RegisterRegistrationIntegrationRequestHandlerFactories(requestHandlerFactories);
        }

        public AsyncRequestExecutorResult Execute(AsyncRequest request)
        {
            tracer = tracerFactory.CreateTracer<RegistrationIntegrationAsyncExecutor>(request.AsyncRequestId);
            if (request.EntityTypeId != EntityTypeEnum.MainClassRegistration || !request.EntityId.HasValue)
                throw new ArgumentException($"Async request has unsupported entity type {request.EntityTypeId} or entity id is null.");

            // loads registration and make initial checking
            var registration = registrationDb.GetClassRegistrationById(request.EntityId.Value,
                ClassRegistrationIncludes.Class | ClassRegistrationIncludes.AddressesCountry);
            if (registration == null)
                throw new ArgumentException($"There is no Class registration with id {request.EntityId.Value}.");
            if (!registrationStateProvider.WasIntegrated(registration))
                throw new InvalidOperationException($"Class registration does not reach state which allows integration with WebEx.");

            // filters WWA registration requests
            if (registrationTypeResolver.IsWwaRegistration(registration.RegistrationTypeId))
            {
                tracer.Warning($"ClassRegistration with id {registration.ClassRegistrationId} is WWA and cannot be processed");
                return new AsyncRequestExecutorResult();
            }
                
            // process request            
            try
            {
                tracer.Info($"Processing of asyncRequestType = {request.AsyncRequestTypeId}");

                // finds handler by class integration type and handles integration request              
                var integrationTypeId = registration.Class.IntegrationTypeId;
                if (!handlerFactories.TryGetValue(integrationTypeId, out var handlerFactory))
                    throw new InvalidOperationException($"There is no handler factory for integration type {integrationTypeId}");

                // creates handler and handles request
                var handler = handlerFactory.CreateRegistrationIntegrationRequestHandler();
                handler.Handle(request, registration);
            }
            catch (Exception e)
            {
                tracer.Error(e, "Registration integration causes error");
                return new AsyncRequestExecutorResult(e, IncidentTypeEnum.OuterSystemError, EntityTypeEnum.MainClassRegistration, registration.ClassRegistrationId);
            }
            
            return new AsyncRequestExecutorResult();
        }

        #region priate methods

        // registers RegistrationIntegrationRequestHandler
        public void RegisterRegistrationIntegrationRequestHandlerFactories(IEnumerable<IRegistrationIntegrationRequestHandlerFactory> requestHandlerFactories)
        {
            foreach (var requestHandlerFactory in requestHandlerFactories)
            {
                handlerFactories.Add(requestHandlerFactory.IntegrationTypeId, requestHandlerFactory);
            }
        }

        #endregion
    }
}
