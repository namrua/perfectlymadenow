using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.Integration.System.NoIntegration
{
    /// <summary>
    /// Factory for NoIntegrationRequestHandler
    /// </summary>
    public class NoIntegrationRequestHandlerFactory : IRegistrationIntegrationRequestHandlerFactory
    {
        private readonly ITracerFactory tracerFactory;

        public IntegrationTypeEnum IntegrationTypeId => IntegrationTypeEnum.NoIntegration;

        public NoIntegrationRequestHandlerFactory(ITracerFactory tracerFactory)
        {
            this.tracerFactory = tracerFactory;
        }

        public IRegistrationIntegrationRequestHandler CreateRegistrationIntegrationRequestHandler()
        {
            return new NoIntegrationRequestHandler(tracerFactory);
        }
    }
}
