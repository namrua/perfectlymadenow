using System.Collections.Generic;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Preferences.Data
{
    /// <summary>
    /// Database layer for preserving system settings
    /// </summary>
    public interface IPreferenceDatabaseLayer
    {
        
        // gets preference for specified key [Cache]
        Preference GetPreferenceByKey(string key, bool useCache = true);      

        // gets all preferences
        List<Preference> GetPreferences(bool useCache = true);

        // updates existing preference
        void Update(string key, string value);

        // inserts or update Preference, return true, if object already exists
        bool InsertUpdate(Preference preference);

    }

}
