using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System
{
    /// <summary>
    /// Provides feeding and checking of allowed registration types for class
    /// </summary>
    public interface IRegistrationTypeFeeder
    {
        HashSet<RegistrationTypeEnum> GetAllowedTypesForAdministrationRegistration(Class cls);

        HashSet<RegistrationTypeEnum> GetAllowedTypesForBatchUploadRegistration(Class cls, RegistrationFormTypeEnum formTypeId);

        HashSet<RegistrationTypeEnum> GetAllowedTypesForPublicRegistration(Class cls, DateTime? nowUtc = null);
        
        void CheckRegistrationTypeForPublicRegistration(Class cls, RegistrationTypeEnum registrationTypeId, DateTime? nowUtc = null);
    }
}
