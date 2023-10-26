using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.WebEx.Contract.Connectors;

namespace AutomationSystem.Main.Core.Integration.System.WebEx
{
    /// <summary>
    /// Factory for WebExRegistrationIntegrationRequestHandler
    /// </summary>
    public class WebExRegistrationIntegrationRequestHandlerFactory : IRegistrationIntegrationRequestHandlerFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IWebExRegistrationConvertor registrationConvertor;

        public IntegrationTypeEnum IntegrationTypeId => IntegrationTypeEnum.WebExProgram;

        public WebExRegistrationIntegrationRequestHandlerFactory(IServiceProvider serviceProvider, IWebExRegistrationConvertor registrationConvertor)
        {
            this.serviceProvider = serviceProvider;
            this.registrationConvertor = registrationConvertor;
        }

        public IRegistrationIntegrationRequestHandler CreateRegistrationIntegrationRequestHandler()
        {
            return new WebExRegistrationIntegrationRequestHandler(serviceProvider.GetService<IIntegrationRequestHandler>(), registrationConvertor);
        }
    }
}
