using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Enums.Data.Models;
using AutomationSystem.Shared.Core.Enums.Data.Extensions;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Enums.Data.EnumDataProviders
{
    /// <summary>
    /// Provides data of core enums
    /// </summary>
    public class CoreEnumDataProvider : IEnumDataProvider
    {

        // constructor
        public CoreEnumDataProvider()
        {
            SupportedEnumTypes = new HashSet<EnumTypeEnum>();
            SupportedEnumTypes.Add(EnumTypeEnum.Language);
        }

        // supported enum types
        public HashSet<EnumTypeEnum> SupportedEnumTypes { get; }

        // gets enum by filter
        public List<IEnumItem> GetItemsByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter)
        {                           
            switch (enumTypeId)
            {
                case EnumTypeEnum.Language:
                    return GetLanguages(filter);
                default:
                    throw new InvalidOperationException($"CoreEnumDataProvider does not support enum type {enumTypeId}");
            }
        }

        #region private methods

        // gets languages
        private List<IEnumItem> GetLanguages(EnumItemFilter filter)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Languages.Filter(filter).ToList();
                return result.Cast<IEnumItem>().ToList();
            }
        }

        #endregion

    }

}
