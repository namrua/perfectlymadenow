using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.Integration.System
{
    /// <summary>
    /// Handles registration integration request
    /// </summary>
    public interface IRegistrationIntegrationRequestHandler
    {
        void Handle(AsyncRequest request, ClassRegistration registration);
    }
}
