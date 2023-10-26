using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Registrations.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    public interface IRegistrationOperationChecker
    {
        bool IsOperationAllowed(RegistrationOperation operation, RegistrationFullState state);

        bool IsOperationDisabled(RegistrationOperation operation, RegistrationFullState state);

        RegistrationFullState CheckOperation(RegistrationOperation operation, ClassRegistration registration);

        void CheckOperation(RegistrationOperation operation, RegistrationFullState state, long registrationId);
    }
}
