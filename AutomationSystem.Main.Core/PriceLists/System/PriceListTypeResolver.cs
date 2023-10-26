using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.PriceLists.System.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.PriceLists.System
{
    /// <summary>
    /// Resolves PriceListType relevant logic and business rules
    /// </summary>
    public class PriceListTypeResolver : IPriceListTypeResolver
    {
        private readonly Dictionary<PriceListTypeEnum, PriceListTypeInfo> priceListMap;

        public PriceListTypeResolver(IRegistrationTypeResolver registrationTypeResolver)
        {
            priceListMap = new Dictionary<PriceListTypeEnum, PriceListTypeInfo>
            {
                [PriceListTypeEnum.Class] = new PriceListTypeInfo(
                    registrationTypeResolver.GetRegistrationTypeByClassCategoryId(ClassCategoryEnum.Class),
                    new HashSet<ClassCategoryEnum> {ClassCategoryEnum.Class}),
                [PriceListTypeEnum.Lecture] = new PriceListTypeInfo(
                    registrationTypeResolver.GetRegistrationTypeByClassCategoryId(ClassCategoryEnum.Lecture),
                    new HashSet<ClassCategoryEnum> {ClassCategoryEnum.Lecture}),
                [PriceListTypeEnum.WwaClass] = new PriceListTypeInfo(
                    registrationTypeResolver.GetRegistrationTypeByClassCategoryId(ClassCategoryEnum.DistanceClass),
                    new HashSet<ClassCategoryEnum> {ClassCategoryEnum.DistanceClass}),
                [PriceListTypeEnum.MaterialClass] = new PriceListTypeInfo(
                    registrationTypeResolver.GetRegistrationTypeByClassCategoryId(ClassCategoryEnum.PrivateMaterialClass),
                    new HashSet<ClassCategoryEnum> {ClassCategoryEnum.PrivateMaterialClass})
            };
        }

        #region business specific methods

        public HashSet<RegistrationTypeEnum> GetRegistrationTypesForPriceList(PriceListTypeEnum priceListTypeId)
        {
            if (!priceListMap.TryGetValue(priceListTypeId, out var priceListInfo))
            {
                throw new ArgumentException($"Unknown PriceList type {priceListTypeId}");
            }

            return new HashSet<RegistrationTypeEnum>(priceListInfo.RegistrationTypesInPriceList);
        }

        public bool IsPriceListAllowedForClassCategoryId(PriceListTypeEnum priceListTypeId, ClassCategoryEnum classCategoryId)
        {
            if (!priceListMap.TryGetValue(priceListTypeId, out var priceListInfo))
            {
                throw new ArgumentException($"Unknown PriceList type {priceListTypeId}");
            }
                
            var result = priceListInfo.AllowedClassCategoryIds.Contains(classCategoryId);
            return result;
        }

        public PriceListTypeEnum GetPriceListTypeIdAllowedForClassCategoryId(ClassCategoryEnum classCategoryId)
        {
            var pair = priceListMap.Where(x => x.Value.AllowedClassCategoryIds.Contains(classCategoryId)).ToList();

            if (!pair.Any())
            {
                throw new ArgumentException($"Unknown Class category {classCategoryId}");
            }

            return pair.First().Key;
        }

        #endregion
    }
}
