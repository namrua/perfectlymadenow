using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders
{
    public class RegistrationTypeFeederForLectures : IRegistrationTypeFeederForClassCategory
    {
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        public RegistrationTypeFeederForLectures(
            IRegistrationTypeResolver registrationTypeResolver)
        {
            this.registrationTypeResolver = registrationTypeResolver;
        }

        public ClassCategoryEnum ClassCategoryId => ClassCategoryEnum.Lecture;

        public HashSet<RegistrationTypeEnum> GetAllowedTypesForAdministrationRegistration(Class cls)
        {
            var result = registrationTypeResolver.GetRegistrationTypeByClassCategoryId(cls.ClassCategoryId);
            result.Add(RegistrationTypeEnum.ApprovedGuest);
            return new HashSet<RegistrationTypeEnum>(result);
        }

        public HashSet<RegistrationTypeEnum> GetAllowedTypesForPublicRegistration(Class cls, DateTime? nowUtc = null)
        {
            var result = registrationTypeResolver.GetRegistrationTypeByClassCategoryId(cls.ClassCategoryId);
            return new HashSet<RegistrationTypeEnum>(result);
        }

        public void CheckRegistrationTypeForPublicRegistration(Class cls, RegistrationTypeEnum registrationTypeId, DateTime? nowUtc = null)
        {
            var allowedForPublicRegistration = GetAllowedTypesForAdministrationRegistration(cls);
            if (!allowedForPublicRegistration.Contains(registrationTypeId))
            {
                throw HomeServiceException
                    .New(HomeServiceErrorType.RegistrationTypeNotAllowed, "Registration type is not allowed")
                    .AddId(classId: cls.ClassId, registrationTypeId: registrationTypeId);
            }
        }
    }
}
