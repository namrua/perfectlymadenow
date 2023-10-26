using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System
{
    public interface IRegistrationStateProvider
    {
        RegistrationState GetRegistrationState(ClassRegistration registration);

        bool? IsReviewed(ClassRegistration registration);

        RegistrationFullState GetRegistrationFullState(ClassRegistration registration);

        bool WasIntegrated(ClassRegistration registration);

    }
}
