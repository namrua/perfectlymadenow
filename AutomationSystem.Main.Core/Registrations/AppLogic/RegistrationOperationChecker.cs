using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Registrations.AppLogic.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    public class RegistrationOperationChecker : IRegistrationOperationChecker
    {
        private readonly IClassOperationChecker classOperationChecker;
        private readonly IRegistrationStateProvider registrationStateProvider;

        public RegistrationOperationChecker(IClassOperationChecker classOperationChecker, IRegistrationStateProvider registrationStateProvider)
        {
            this.classOperationChecker = classOperationChecker;
            this.registrationStateProvider = registrationStateProvider;
        }

        public bool IsOperationAllowed(RegistrationOperation operation, RegistrationFullState state)
        {
            var result = false;
            switch (operation)
            {
                case RegistrationOperation.DeleteRegistration:
                    result = classOperationChecker.IsOperationAllowed(ClassOperation.EditRegistration, state.ClassState)
                             && (state.RegistrationState == RegistrationState.New && state.ApprovementTypeId == ApprovementTypeEnum.ManualApprovement
                             || state.RegistrationState == RegistrationState.Temporary);
                    break;

                case RegistrationOperation.ApproveRegistration:
                    result = classOperationChecker.IsOperationAllowed(ClassOperation.EditRegistration, state.ClassState)
                             && state.RegistrationState == RegistrationState.New
                             && state.ApprovementTypeId != ApprovementTypeEnum.None;
                    break;

                case RegistrationOperation.CancelRegistration:
                    result = !IsOperationAllowed(RegistrationOperation.DeleteRegistration, state)
                             && state.RegistrationState != RegistrationState.Canceled;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
            return result;
        }

        public bool IsOperationDisabled(RegistrationOperation operation, RegistrationFullState state)
        {
            var isAllowed = IsOperationAllowed(operation, state);
            return !isAllowed;
        }

        public RegistrationFullState CheckOperation(RegistrationOperation operation, ClassRegistration registration)
        {
            var result = registrationStateProvider.GetRegistrationFullState(registration);
            CheckOperation(operation, result, registration.ClassRegistrationId);
            return result;
        }

        public void CheckOperation(RegistrationOperation operation, RegistrationFullState state, long registrationId)
        {
            if (IsOperationDisabled(operation, state))
                throw new InvalidOperationException($"Operaton {operation} is not allowed for Class registration with id {registrationId}.");
        }

    }

}
