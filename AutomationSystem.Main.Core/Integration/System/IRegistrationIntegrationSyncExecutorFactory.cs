using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.Integration.System
{
    /// <summary>
    /// Factory for registration integration sync executor
    /// </summary>
    public interface IRegistrationIntegrationSyncExecutorFactory
    { 
        IntegrationTypeEnum IntegrationTypeId { get; }
        IRegistrationIntegrationSyncExecutor CreateRegistrationIntegrationSyncExecutor();
    }
}
