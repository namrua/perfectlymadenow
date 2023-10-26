using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Integration.System;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.AsyncRequestExecutors
{
    /// <summary>
    /// Factory for RegistrationIntegrationAsyncExecutorFactory
    /// </summary>
    public class RegistrationIntegrationAsyncExecutorFactory : IAsyncRequestExecutorFactory
    {
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IEnumerable<IRegistrationIntegrationRequestHandlerFactory> requestHandlerFactories;
        private readonly ITracerFactory tracerFactory;
        private readonly IRegistrationStateProvider registrationStateProvider;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public HashSet<AsyncRequestTypeEnum> SupportedAsyncRequestTypes => new HashSet<AsyncRequestTypeEnum>
        {
            AsyncRequestTypeEnum.AddToOuterSystem,
            AsyncRequestTypeEnum.RemoveFromOuterSystem,
            AsyncRequestTypeEnum.UpdateOuterSystemState,
            AsyncRequestTypeEnum.SyncOuterSystemState,
            AsyncRequestTypeEnum.SendOuterSystemInvitation
        };

        public RegistrationIntegrationAsyncExecutorFactory(
            IRegistrationDatabaseLayer registrationDb,
            IEnumerable<IRegistrationIntegrationRequestHandlerFactory> requestHandlerFactories,
            ITracerFactory tracerFactory,
            IRegistrationStateProvider registrationStateProvider,
            IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationDb = registrationDb;
            this.requestHandlerFactories = requestHandlerFactories;
            this.tracerFactory = tracerFactory;
            this.registrationStateProvider = registrationStateProvider;
            this.registrationTypeResolver = registrationTypeResolver;
        }

        public IAsyncRequestExecutor CreateAsyncRequestExecutor()
        {
            var result = new RegistrationIntegrationAsyncExecutor(registrationDb, requestHandlerFactories, tracerFactory, registrationStateProvider, registrationTypeResolver);
            return result;
        }
    }
}
