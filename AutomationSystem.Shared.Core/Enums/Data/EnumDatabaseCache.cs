using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Contract.Database;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Shared.Core.Enums.Data
{
    /// <summary>
    /// Enum database layer cached
    /// </summary>
    public class EnumDatabaseCache : IEnumDatabaseLayer
    {

        // private components
        private readonly IEnumDatabaseLayer enumDb;
        private readonly Dictionary<EnumTypeEnum, IDataCache<int, IEnumItem>> caches;

        // constructor
        public EnumDatabaseCache(IEnumDatabaseLayer enumDb, IEnumerable<IEnumDataProvider> enumDataProviders)
        {
            this.enumDb = enumDb;
            caches = new Dictionary<EnumTypeEnum, IDataCache<int, IEnumItem>>();
            RegisterEnumDataProviders(enumDataProviders);
        }
       

        // gets all enum types
        public List<EnumType> GetEnumTypes()
        {
            return enumDb.GetEnumTypes();
        }

        // gets enum type by id
        public EnumType GetEnumTypeById(EnumTypeEnum enumTypeId)
        {
            return enumDb.GetEnumTypeById(enumTypeId);
        }


        // get enum by id
        public IEnumItem GetItemById(EnumTypeEnum enumTypeId, int id)
        {
            var cache = GetCache(enumTypeId);
            var result = cache.GetItemById(id);
            return result;
        }

        // gets enum by filter [cached]
        public List<IEnumItem> GetItemsByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter)
        {
            var cache = GetCache(enumTypeId);
            var result = (filter?.Id != null)
                ? cache.GetListByKeys(new[] {filter.Id.Value})
                : cache.GetListByFilter();
            return result;
        }

        // gets enum map by filter [cached]
        public Dictionary<int, IEnumItem> GetMapByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter)
        {
            return GetItemsByFilter(enumTypeId, filter).ToDictionary(x => x.Id, y => y);
        }



        #region private methods

        // registers enum data providers
        public void RegisterEnumDataProviders(IEnumerable<IEnumDataProvider> enumDataProviders)
        {
            foreach (var enumDataProvider in enumDataProviders)
            {
                foreach (var enumTypeId in enumDataProvider.SupportedEnumTypes)
                {
                    var feeder = new EnumCacheFeeder(enumDataProvider, enumTypeId);
                    caches[enumTypeId] = new DataCache<int, IEnumItem>(feeder, x => x.Id);
                }
            }
        }

        // gets cache
        private IDataCache<int, IEnumItem> GetCache(EnumTypeEnum enumTypeId)
        {
            IDataCache<int, IEnumItem> cache;
            if (!caches.TryGetValue(enumTypeId, out cache))
                throw new InvalidOperationException(string.Format("does not support enum type {0}", enumTypeId));
            return cache;
        }

        #endregion

    }

}
