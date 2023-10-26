using System;
using System.Collections.Generic;
using System.Linq;
using PerfectlyMadeInc.Helpers.Contract.Database;

namespace PerfectlyMadeInc.Helpers.Database
{
    /// <summary>
    /// Provides access to data
    /// </summary>
    public class DataCache<TKey, TObject> : IDataCache<TKey, TObject>
    {

        // private fields
        private readonly ICacheFeeder<TObject> feeder;
        private readonly Func<TObject, TKey> idSelector;
        private List<TObject> list;
        private Dictionary<TKey, TObject> map;

        // constructor
        public DataCache(ICacheFeeder<TObject> feeder, Func<TObject, TKey> idSelector)
        {
            this.feeder = feeder;
            this.idSelector = idSelector;
            list = new List<TObject>();
            map = new Dictionary<TKey, TObject>();
            IsExpired = true;
            CacheRepository.RegisterExpirator(this);
        }


        // determines whether cache is expired
        public bool IsExpired { get; private set; }

        // gets item by key
        public TObject GetItemById(TKey key)
        {
            lock (this)
            {
                Refresh();
                TObject result;
                map.TryGetValue(key, out result);
                return result;
            }
        }

        // gets list of items 
        public List<TObject> GetListByKeys(IEnumerable<TKey> ids)
        {
            lock (this)
            {
                Refresh();                
                var result = ids.Select(GetItemById).Where(x => x != null).ToList();
                return result;
            }
        }
     
        // gets list of items by filter (null gets all
        public List<TObject> GetListByFilter(Func<TObject, bool> filter = null)
        {
            lock (this)
            {
                Refresh();
                if (filter == null)
                    return list.ToList();
                var result = list.Where(filter).ToList();
                return result;
            }
        }

        // get list of items by query
        public List<TObject> GetListByQuery(Func<IQueryable<TObject>, IQueryable<TObject>> query)
        {
            lock (this)
            {
                Refresh();
                var result = query(list.AsQueryable()).ToList();
                return result;
            }
        }
        

        // gets dictionary of items (null gets all)
        public Dictionary<TKey, TObject> GetMap(Func<TObject, bool> filter = null)
        {
            lock (this)
            {
                Refresh();
                if (filter == null)
                    return map.ToDictionary(x => x.Key, y => y.Value);
                var result = list.Where(filter).ToDictionary(x => idSelector(x), y => y);
                return result;
            }
        }

        // sets as expired
        public void SetAsExpired()
        {
            lock (this)
            {
                IsExpired = true;
            }
        }

        #region private fields

        // refresh
        private void Refresh()
        {
            if (!IsExpired) return;
            list = feeder.GetData();
            map = list.ToDictionary(x => idSelector(x), y => y);
            IsExpired = false;
        }

        #endregion

    }

}
