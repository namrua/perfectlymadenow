using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationSystem.Base.Contract.Enums
{

    /// <summary>
    /// Encapsulates EnumItem extensions
    /// </summary>
    public static class EnumItemExtensions
    {

        // filters set of enum items by is set
        public static IEnumerable<IEnumItem> FilterByIdSet<T>(this IEnumerable<IEnumItem> collection, HashSet<T> allowedIds, Func<int, T> idMapper)
        {
            var result = collection.Where(x => allowedIds.Contains(idMapper(x.Id)));
            return result;
        }

    }

}
