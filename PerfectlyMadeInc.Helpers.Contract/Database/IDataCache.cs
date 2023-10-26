using System;
using System.Collections.Generic;
using System.Linq;

namespace PerfectlyMadeInc.Helpers.Contract.Database
{
    /// <summary>
    /// Data cache
    /// </summary>
    /// <typeparam name="TObject">object type</typeparam>
    /// <typeparam name="TKey">key type</typeparam>
    public interface IDataCache<TKey, TObject> : ICacheExpirator
    {
        
        // determines whether cache is expired
        bool IsExpired { get; }

        // gets item by key
        TObject GetItemById(TKey key);

        // gets list of items 
        List<TObject> GetListByKeys(IEnumerable<TKey> ids);

        // gets list of items by filter (null gets all)
        List<TObject> GetListByFilter(Func<TObject, bool> filter = null);

        // get list of items by query
        List<TObject> GetListByQuery(Func<IQueryable<TObject>, IQueryable<TObject>> query);

            // gets dictionary of items (null gets all)
        Dictionary<TKey, TObject> GetMap(Func<TObject, bool> filter = null);        

    }

}
