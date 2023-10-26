using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.TimeZones.System;

namespace AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders
{
    public class RegistrationTypeFeederForClasses : IRegistrationTypeFeederForClassCategory
    {
        private readonly ITimeZoneService timeZoneService;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly IClassTypeResolver classTypeResolver;

        public RegistrationTypeFeederForClasses(
            ITimeZoneService timeZoneService,
            IRegistrationTypeResolver registrationTypeResolver,
            IClassTypeResolver classTypeResolver)
        {
            this.timeZoneService = timeZoneService;
            this.registrationTypeResolver = registrationTypeResolver;
            this.classTypeResolver = classTypeResolver;
        }

        public ClassCategoryEnum ClassCategoryId => ClassCategoryEnum.Class;

        public HashSet<RegistrationTypeEnum> GetAllowedTypesForAdministrationRegistration(Class cls)
        {
            var result = GetBaseRegistrationTypeSet(cls);
            result.Add(RegistrationTypeEnum.ApprovedGuest);
            return result;
        }

        public HashSet<RegistrationTypeEnum> GetAllowedTypesForPublicRegistration(Class cls, DateTime? nowUtc = null)
        {
            var result = GetBaseRegistrationTypeSet(cls);

            nowUtc = (nowUtc ?? DateTime.UtcNow).AddMinutes(15);    // prevents creating of pre-registration registration that will cause expired exception
            result = ApplyTemporaryFilter(result, cls, nowUtc, false, out _);
            return result;
        }
        
        public void CheckRegistrationTypeForPublicRegistration(Class cls, RegistrationTypeEnum registrationTypeId, DateTime? nowUtc = null)
        {
            var result = GetAllowedTypesForAdministrationRegistration(cls);
            result = ApplyTemporaryFilter(result, cls, nowUtc, true, out var isPostRegistration);
            if (result.Contains(registrationTypeId)) return;
            
            if (IsPreRegistrationExpiration(registrationTypeId, isPostRegistration))
                throw HomeServiceException
                    .New(HomeServiceErrorType.PreRegistrationClosed, "Pre registration was closed")
                    .AddId(classId: cls.ClassId, registrationTypeId: registrationTypeId);

            throw HomeServiceException.New(HomeServiceErrorType.RegistrationTypeNotAllowed,
                    "Registration type is not allowed")
                .AddId(classId: cls.ClassId, registrationTypeId: registrationTypeId);
        }

        #region private methods
        
        private HashSet<RegistrationTypeEnum> GetBaseRegistrationTypeSet(Class cls)
        {
            var classTypeInfo = classTypeResolver.GetClassTypeInfo(cls.ClassTypeId);
            var result = registrationTypeResolver.GetRegistrationTypeByClassCategoryIdAndTopic(cls.ClassCategoryId, classTypeInfo.Topic);
            result = result.Where(x => x != RegistrationTypeEnum.WWA || cls.IsWwaFormAllowed).ToList();
            return new HashSet<RegistrationTypeEnum>(result);
        }

        private HashSet<RegistrationTypeEnum> ApplyTemporaryFilter(
            IEnumerable<RegistrationTypeEnum> registrationTypeIds,
            Class cls,
            DateTime? nowUtc,
            bool keepExpensive,
            out bool isPostRegistration)
        {
            isPostRegistration = false;
            nowUtc = nowUtc ?? DateTime.UtcNow;
            var weekOfClassStartUtc = GetWeekOfClassStartUtc(cls);           
            var result = new List<RegistrationTypeEnum>();
            foreach (var registrationTypeId in registrationTypeIds)
            {
                switch (registrationTypeId)
                {
                    case RegistrationTypeEnum.NewAdult:
                        if (nowUtc < weekOfClassStartUtc)
                            result.Add(registrationTypeId);
                        break;
                        
                    case RegistrationTypeEnum.NewAdultWeekOfClass:
                        if (weekOfClassStartUtc <= nowUtc || keepExpensive)
                        {
                            isPostRegistration = true;
                            result.Add(registrationTypeId);
                        }
                        break;
                        
                    default:
                        result.Add(registrationTypeId);
                        break;
                    
                }
            }
            return new HashSet<RegistrationTypeEnum>(result);
        }
        
        private DateTime GetWeekOfClassStartUtc(Class cls)
        {
            var weekOfClassDayTemplate = cls.EventStart.AddDays(-(int)cls.EventStart.DayOfWeek);
            var weekOfClassDay = new DateTime(weekOfClassDayTemplate.Year, weekOfClassDayTemplate.Month, weekOfClassDayTemplate.Day);
            var weekOfClassStartUtc = timeZoneService.GetUtcDateTime(weekOfClassDay, cls.TimeZoneId);
            return weekOfClassStartUtc;
        }

        private bool IsPreRegistrationExpiration(RegistrationTypeEnum registrationTypeId, bool isPostRegistration)
        {
            return isPostRegistration && registrationTypeId == RegistrationTypeEnum.NewAdult;
        }

        #endregion
    }
}
