using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System
{
    public interface IRegistrationCommandService
    {
        void ApproveRegistration(long registrationId);

        void ApproveRegistration(ClassRegistration registration);
    }
}
