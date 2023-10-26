using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders
{
    public class RegistrationTypeFeederForPrivateMaterialClasses : IRegistrationTypeFeederForClassCategory
    {
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public RegistrationTypeFeederForPrivateMaterialClasses(
            IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationTypeResolver = registrationTypeResolver;
        }

        public ClassCategoryEnum ClassCategoryId => ClassCategoryEnum.PrivateMaterialClass;

        public HashSet<RegistrationTypeEnum> GetAllowedTypesForAdministrationRegistration(Class cls)
        {
            var result = registrationTypeResolver.GetRegistrationTypeByClassCategoryId(cls.ClassCategoryId);
            return new HashSet<RegistrationTypeEnum>(result);
        }

        public HashSet<RegistrationTypeEnum> GetAllowedTypesForPublicRegistration(Class cls, DateTime? nowUtc = null)
        {
            return new HashSet<RegistrationTypeEnum>();
        }

        public void CheckRegistrationTypeForPublicRegistration(Class cls, RegistrationTypeEnum registrationTypeId, DateTime? nowUtc = null)
        {
            throw HomeServiceException
                .New(HomeServiceErrorType.RegistrationTypeNotAllowed, "Registration type is not allowed")
                .AddId(classId: cls.ClassId, registrationTypeId: registrationTypeId);
        }
    }
}
