using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.Integration.System.NoIntegration
{
    /// <summary>
    /// Factory for NoIntegrationSyncExecutor
    /// </summary>
    public class NoIntegrationSyncExecutorFactory : IRegistrationIntegrationSyncExecutorFactory
    {
        private readonly ITracerFactory tracerFactory;

        public NoIntegrationSyncExecutorFactory(ITracerFactory tracerFactory)
        {
            this.tracerFactory = tracerFactory;
        }

        public IntegrationTypeEnum IntegrationTypeId => IntegrationTypeEnum.NoIntegration;

        public IRegistrationIntegrationSyncExecutor CreateRegistrationIntegrationSyncExecutor()
        {
            return new NoIntegrationSyncExecutor(tracerFactory);
        }
    }
}
