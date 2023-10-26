using System.Collections.Generic;
using PerfectlyMadeInc.Helpers.Contract.Database.Models;

namespace PerfectlyMadeInc.Helpers.Contract.Database
{
    /// <summary>
    /// Resolves set update strategy
    /// </summary>    
    public interface ISetUpdateResolver<T>
    {
        
        // resolves strategy to update set
        SetUpdateResolverStrategy<T> ResolveStrategy(IEnumerable<T> currentItems, IEnumerable<T> newItems);

    }

}
