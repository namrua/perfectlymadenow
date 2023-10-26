using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.WebEx.Contract.Connectors;

namespace AutomationSystem.Main.Core.Integration.System.WebEx
{
    /// <summary>
    /// Factory for WebExRegistrationIntegrationSyncExecutor
    /// </summary>
    public class WebExRegistrationIntegrationSyncExecutorFactory : IRegistrationIntegrationSyncExecutorFactory
    {
        private readonly ITracerFactory tracerFactory;
        private readonly IServiceProvider serviceProvider;
        private readonly IWebExRegistrationConvertor registrationConvertor;

        public IntegrationTypeEnum IntegrationTypeId => IntegrationTypeEnum.WebExProgram;

        public WebExRegistrationIntegrationSyncExecutorFactory(ITracerFactory tracerFactory, IServiceProvider serviceProvider, IWebExRegistrationConvertor registrationConvertor)
        {
            this.tracerFactory = tracerFactory;
            this.serviceProvider = serviceProvider;
            this.registrationConvertor = registrationConvertor;
        }

        public IRegistrationIntegrationSyncExecutor CreateRegistrationIntegrationSyncExecutor()
        {
            return new WebExRegistrationIntegrationSyncExecutor(serviceProvider.GetService<IIntegrationSyncExecutor>(), tracerFactory, registrationConvertor);
        }
    }
}
