using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders
{
    public interface IRegistrationTypeFeederForClassCategory
    {
        ClassCategoryEnum ClassCategoryId { get; }

        HashSet<RegistrationTypeEnum> GetAllowedTypesForAdministrationRegistration(Class cls);

        HashSet<RegistrationTypeEnum> GetAllowedTypesForPublicRegistration(Class cls, DateTime? nowUtc = null);

        void CheckRegistrationTypeForPublicRegistration(Class cls, RegistrationTypeEnum registrationTypeId, DateTime? nowUtc = null);
    }
}
