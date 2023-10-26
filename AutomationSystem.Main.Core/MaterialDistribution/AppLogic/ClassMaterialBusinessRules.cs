using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    /// <summary>
    /// Defines class material business rules
    /// </summary>
    public class ClassMaterialBusinessRules : IClassMaterialBusinessRules
    {
        public const int NativeMaterialLockAfterClassEndDateInDays = 2;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        private readonly HashSet<PersonRoleTypeEnum> materialSupportingPersonRoles;

        public ClassMaterialBusinessRules(IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationTypeResolver = registrationTypeResolver;

            materialSupportingPersonRoles = new HashSet<PersonRoleTypeEnum>
            {
                PersonRoleTypeEnum.GuestInstructor,
                PersonRoleTypeEnum.Instructor
            };
        }

        public HashSet<PersonRoleTypeEnum> GetMaterialSupportingPersonRoles()
        {
            return new HashSet<PersonRoleTypeEnum>(materialSupportingPersonRoles);
        }

        public bool AreMaterialsSupported(RegistrationTypeEnum registrationTypeId)
        {
            var result = GetMaterialSupportingRegistrationTypes().Contains(registrationTypeId);
            return result;
        }

        public HashSet<RegistrationTypeEnum> GetMaterialSupportingRegistrationTypes()
        {
            var result = registrationTypeResolver.GetNewRegistrationTypes();
            result.Add(RegistrationTypeEnum.ApprovedGuest);
            result.Add(RegistrationTypeEnum.MaterialRegistration);
            return result;
        }

        public bool IsLockedByClassEndDate(DateTime eventEndUtc, DateTime utcNow)
        {
            var utcDeadline = eventEndUtc.AddDays(NativeMaterialLockAfterClassEndDateInDays);
            return utcDeadline < utcNow;
        }
    }
}
