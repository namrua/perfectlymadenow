using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.Integration.System
{
    /// <summary>
    /// Factory for registration integration request handler
    /// </summary>
    public interface IRegistrationIntegrationRequestHandlerFactory
    {
        IntegrationTypeEnum IntegrationTypeId { get; }
        IRegistrationIntegrationRequestHandler CreateRegistrationIntegrationRequestHandler();
    }
}
