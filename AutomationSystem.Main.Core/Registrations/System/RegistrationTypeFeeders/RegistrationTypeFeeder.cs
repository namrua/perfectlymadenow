using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders
{
    /// <summary>
    /// Provides feeding and checking of allowed registration types for class 
    /// </summary>
    public class RegistrationTypeFeeder : IRegistrationTypeFeeder
    {
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly Dictionary<ClassCategoryEnum, IRegistrationTypeFeederForClassCategory> feedersMap;

        public RegistrationTypeFeeder(
            IRegistrationTypeResolver registrationTypeResolver,
            IEnumerable<IRegistrationTypeFeederForClassCategory> feedersForClassCategories)
        {
            this.registrationTypeResolver = registrationTypeResolver;
            feedersMap = feedersForClassCategories.ToDictionary(x => x.ClassCategoryId);
        }
        
        public HashSet<RegistrationTypeEnum> GetAllowedTypesForAdministrationRegistration(Class cls)
        {
            var feeder = GetFeederByClassCategoryId(cls.ClassCategoryId);
            var result = feeder.GetAllowedTypesForAdministrationRegistration(cls);
            return result;
        }

        public HashSet<RegistrationTypeEnum> GetAllowedTypesForBatchUploadRegistration(Class cls, RegistrationFormTypeEnum formTypeId)
        {
            var result = GetAllowedTypesForAdministrationRegistration(cls)
                .Where(x => registrationTypeResolver.GetRegistrationFormType(x) == formTypeId);
            return new HashSet<RegistrationTypeEnum>(result);
        }

        public HashSet<RegistrationTypeEnum> GetAllowedTypesForPublicRegistration(Class cls, DateTime? nowUtc = null)
        {
            var feeder = GetFeederByClassCategoryId(cls.ClassCategoryId);
            var result = feeder.GetAllowedTypesForPublicRegistration(cls, nowUtc);
            return result;
        }
        
        public void CheckRegistrationTypeForPublicRegistration(Class cls, RegistrationTypeEnum registrationTypeId, DateTime? nowUtc = null)
        {
            var feeder = GetFeederByClassCategoryId(cls.ClassCategoryId);
            feeder.CheckRegistrationTypeForPublicRegistration(cls, registrationTypeId, nowUtc);
        }

        #region private methods

        private IRegistrationTypeFeederForClassCategory GetFeederByClassCategoryId(ClassCategoryEnum classCategoryId)
        {
            if (!feedersMap.TryGetValue(classCategoryId, out var feeder))
            {
                throw new InvalidOperationException($"Cannot obtain registration type feeder for class category {classCategoryId}.");
            }

            return feeder;
        }

        #endregion
    }
}
