using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Enums.Data.Extensions;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;

namespace AutomationSystem.Main.Core.Enums.Data
{
    /// <summary>
    /// Provides data of lightversion  enums
    /// </summary>
    public class MainEnumDataProvider : IEnumDataProvider
    {

        // constructor
        public MainEnumDataProvider()
        {
            SupportedEnumTypes = new HashSet<EnumTypeEnum>();
            
            SupportedEnumTypes.Add(EnumTypeEnum.Country);
            SupportedEnumTypes.Add(EnumTypeEnum.Currency);            
            SupportedEnumTypes.Add(EnumTypeEnum.TimeZone);

            SupportedEnumTypes.Add(EnumTypeEnum.MainClassType);
            SupportedEnumTypes.Add(EnumTypeEnum.MainRegistrationType);
        }

        // supported enum types
        public HashSet<EnumTypeEnum> SupportedEnumTypes { get; }

        // gets enum by filter
        public List<IEnumItem> GetItemsByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter)
        {
            switch (enumTypeId)
            {
                // enum
                case EnumTypeEnum.Country:
                    return GetCountries(filter);
                case EnumTypeEnum.Currency:
                    return GetCurrencies(filter);
                case EnumTypeEnum.TimeZone:
                    return GetTimeZones(filter);

                // main
                case EnumTypeEnum.MainClassType:
                    return GetClassTypes(filter);
                case EnumTypeEnum.MainRegistrationType:
                    return GetRegistrationTypes(filter);
                
                default:
                    throw new InvalidOperationException(
                        $"LightEnumDataProvider does not support enum type {enumTypeId}");
            }
        }

        #region private methods


        // gets class types
        private List<IEnumItem> GetClassTypes(EnumItemFilter filter)
        {
            using (var context = new MainEntities())
            {
                var result = context.ClassTypes.Filter(filter).ToList();
                return result.Cast<IEnumItem>().ToList();
            }
        }

        // gets countries
        private List<IEnumItem> GetCountries(EnumItemFilter filter)
        {
            using (var context = new MainEntities())
            {
                var result = context.Countries.Filter(filter).ToList();                                
                return result.Cast<IEnumItem>().ToList();
            }
        }

        // gets currencies
        private List<IEnumItem> GetCurrencies(EnumItemFilter filter)
        {
            using (var context = new MainEntities())
            {
                var result = context.Currencies.Filter(filter).ToList();
                return result.Cast<IEnumItem>().ToList();
            }
        }

        // gets registration types
        private List<IEnumItem> GetRegistrationTypes(EnumItemFilter filter)
        {
            using (var context = new MainEntities())
            {
                var result = context.RegistrationTypes.Filter(filter).ToList();
                return result.Cast<IEnumItem>().ToList();
            }
        }

        // gets timezones
        private List<IEnumItem> GetTimeZones(EnumItemFilter filter)
        {
            using (var context = new MainEntities())
            {
                var result = context.TimeZones.Filter(filter).ToList();
                return result.Cast<IEnumItem>().ToList();
            }
        }

        #endregion

    }

}
