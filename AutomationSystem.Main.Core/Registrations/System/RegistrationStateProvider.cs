using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Model;
using System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.Registrations.System
{
    public class RegistrationStateProvider : IRegistrationStateProvider
    {
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public RegistrationStateProvider(IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationTypeResolver = registrationTypeResolver;
        }

        public RegistrationState GetRegistrationState(ClassRegistration registration)
        {
            if (registration.IsTemporary)
                return RegistrationState.Temporary;
            if (registration.IsCanceled)
                return RegistrationState.Canceled;
            if (registration.IsApproved)
                return RegistrationState.Approved;
            return RegistrationState.New;
        }

        // determines whether is registration reviewed, returns null whether registration is not needed to be reviewed
        public bool? IsReviewed(ClassRegistration registration)
        {
            if (registration.Class == null)
            {
                throw new InvalidOperationException("Class is not included into ClassRegistration object.");
            }

            if (registrationTypeResolver.NeedsReview(registration.RegistrationTypeId, registration.Class.ClassTypeId))
            {
                return registration.IsReviewed;
            }

            return null;
        }


        public RegistrationFullState GetRegistrationFullState(ClassRegistration registration)
        {
            if (registration.Class == null)
                throw new InvalidOperationException("Class is not included into ClassRegistration object.");

            var result = new RegistrationFullState
            {
                ClassState = ClassConvertor.GetClassState(registration.Class),
                ApprovementTypeId = registration.ApprovementTypeId,
                RegistrationState = GetRegistrationState(registration),
                IsReviewed = IsReviewed(registration)
            };
            return result;
        }

        // determines whether registration was integrated ==> was approved in the past or now (approved = true, canceled = true/false)
        public bool WasIntegrated(ClassRegistration registration)
        {
            return registration.IsApproved;
        }
    }
}
