using System.Collections.Generic;

namespace PerfectlyMadeInc.Helpers.Contract.Database
{
    /// <summary>
    /// Provides data to cache
    /// </summary>   
    public interface ICacheFeeder<T>
    {

        // returns data for cache
        List<T> GetData();

    }

}
