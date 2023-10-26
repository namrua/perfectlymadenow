using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Preferences.Data;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.Helpers.Contract.Database;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Shared.Core.Preferences.Data
{
    /// <summary>
    /// Database layer for preserving system settings (cache)
    /// </summary>
    public class PreferenceDatabaseLayerCache : IPreferenceDatabaseLayer, ICacheFeeder<Preference>
    {

        // private components
        private readonly IPreferenceDatabaseLayer preferenceDb;
        private readonly IDataCache<string, Preference> cache;


        // constructor
        public PreferenceDatabaseLayerCache(IPreferenceDatabaseLayer preferenceDb)
        {
            this.preferenceDb = preferenceDb;
            cache = new DataCache<string, Preference>(this, x => x.Key);
        }

        // gets preference for specified key [Cache]
        public Preference GetPreferenceByKey(string key, bool useCache = true)
        {
            if (!useCache)
                return preferenceDb.GetPreferenceByKey(key, false);
            var result = cache.GetItemById(key);
            return result;
        }

        // gets all preferences
        public List<Preference> GetPreferences(bool useCache = true)
        {
            if (!useCache)
                return preferenceDb.GetPreferences(false);
            var result = cache.GetListByFilter();
            return result;
        }

        // updates existing preference
        public void Update(string key, string value)
        {
            preferenceDb.Update(key, value);
            cache.SetAsExpired();
        }

        // inserts or update Preference, return true, if object already exists
        public bool InsertUpdate(Preference preference)
        {
            var result = preferenceDb.InsertUpdate(preference);
            cache.SetAsExpired();
            return result;
        }

        // feeder for language cache
        List<Preference> ICacheFeeder<Preference>.GetData()
        {
            return preferenceDb.GetPreferences();
        }

    }

}
