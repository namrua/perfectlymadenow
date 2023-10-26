using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Enums.Data.Models;

namespace AutomationSystem.Shared.Contract.Enums.Data
{
    /// <summary>
    /// Provides data of enums
    /// </summary>
    public interface IEnumDataProvider
    {

        // supported enum types
        HashSet<EnumTypeEnum> SupportedEnumTypes { get; }

            // gets enum by filter
        List<IEnumItem> GetItemsByFilter(EnumTypeEnum enumTypeId, EnumItemFilter filter);

    }


}
