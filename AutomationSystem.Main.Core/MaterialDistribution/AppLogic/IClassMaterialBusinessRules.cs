using AutomationSystem.Base.Contract.Enums;
using System;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.MaterialDistribution.AppLogic
{
    public interface IClassMaterialBusinessRules
    {
        HashSet<PersonRoleTypeEnum> GetMaterialSupportingPersonRoles();

        bool AreMaterialsSupported(RegistrationTypeEnum registrationTypeId);

        HashSet<RegistrationTypeEnum> GetMaterialSupportingRegistrationTypes();

        bool IsLockedByClassEndDate(DateTime eventEndUtc, DateTime utcNow);
    }
}
