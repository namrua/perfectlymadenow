using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using PerfectlyMadeInc.Helpers.Contract.Database;

namespace AutomationSystem.Shared.Core.Enums.Data
{
    /// <summary>
    /// Feeder for enum cache
    /// </summary>
    public class EnumCacheFeeder : ICacheFeeder<IEnumItem>
    {

        // private components
        private readonly IEnumDataProvider provider;
        private readonly EnumTypeEnum enumTypeId;

        // constructor
        public EnumCacheFeeder(IEnumDataProvider provider, EnumTypeEnum enumTypeId)
        {
            this.provider = provider;
            this.enumTypeId = enumTypeId;
        }

        // gets data for cache
        public List<IEnumItem> GetData()
        {
            var result = provider.GetItemsByFilter(enumTypeId, null);
            return result;
        }

    }

}
