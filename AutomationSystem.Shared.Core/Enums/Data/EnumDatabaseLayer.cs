using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Enums.Data
{
    /// <summary>
    /// Enum database layer
    /// </summary>
    public class EnumDatabaseLayer : IEnumDatabaseLayer
    {
        // private fields
        private readonly Dictionary<EnumTypeEnum, IEnumDataProvider> enumDataProvidersMap;

        // constructor
        public EnumDatabaseLayer(IEnumerable<IEnumDataProvider> enumDataProviders)
        {
            enumDataProvidersMap = new Dictionary<EnumTypeEnum, IEnumDataProvider>();
            RegisterEnumDataProviders(enumDataProviders);
        }

        // gets all enum types
        public List<EnumType> GetEnumTypes()
        {
            using (var context = new CoreEntities())
            {
                var result = context.EnumTypes.ToList();
                return result;
            }
        }

        // gets enum type by id
        public EnumType GetEnumTypeById(EnumTypeEnum enumTypeId)
        {
            using (var context = new CoreEntities())
            {
                var reuslt = context.EnumTypes.FirstOrDefault(x => x.EnumTypeId == enumTypeId);
                return reuslt;
            }
        }


        // get enum by id
        public IEnumItem GetItemById(EnumTypeEnum enumTypeId, int id)
        {
            var resultList = GetItemsByFilter(enumTypeId, new EnumItemFilter(id));
            return resultList.FirstOrDefault();               
        }


        // gets enum by filter
        public List<IEnumItem> GetItemsByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter)
        {
            IEnumDataProvider provider;
            if (!enumDataProvidersMap.TryGetValue(enumTypeId, out provider))
                throw new InvalidOperationException($"does not support enum type {enumTypeId}");
            var result = provider.GetItemsByFilter(enumTypeId, filter);
            return result;
        }

        // gets enum map by filter
        public Dictionary<int, IEnumItem> GetMapByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter)
        {
            return GetItemsByFilter(enumTypeId, filter).ToDictionary(x => x.Id);
        }

        #region private methods

        // registers enum data providers
        private void RegisterEnumDataProviders(IEnumerable<IEnumDataProvider> enumDataProviders)
        {
            foreach (var enumDataProvider in enumDataProviders)
            {
                foreach (var enumTypeId in enumDataProvider.SupportedEnumTypes)
                {
                    enumDataProvidersMap[enumTypeId] = enumDataProvider;
                }
            }
        }

        #endregion

    }

}
