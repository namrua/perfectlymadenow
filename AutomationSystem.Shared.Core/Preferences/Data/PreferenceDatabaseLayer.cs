using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Shared.Contract.Preferences.Data;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Preferences.Data
{   
    /// <summary>
    /// Database layer for preserving system settings
    /// </summary>
    public class PreferenceDatabaseLayer : IPreferenceDatabaseLayer
    {

        // gets preference for specified key
        public Preference GetPreferenceByKey(string key, bool useCache = true)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Preferences.Active().FirstOrDefault(x => x.Key == key);
                return result;
            }
        }

        // gets all preferences
        public List<Preference> GetPreferences(bool useCache = true)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Preferences.Active().ToList();
                return result;
            }
        }


        // updates existing preference
        public void Update(string key, string value)
        {
            using (var context = new CoreEntities())
            {
                var preference = context.Preferences.Active().First(x => x.Key == key);
                preference.Value = value;
                context.SaveChanges();
            }
        }

        // inserts or update Preference, return true, if object already exists
        public bool InsertUpdate(Preference preference)
        {
            using (var context = new CoreEntities())
            {
                var toUpdate = context.Preferences.Active().FirstOrDefault(x => x.Key == preference.Key);
                if (toUpdate != null)               
                {
                    // updates object
                    toUpdate.Value = preference.Value;
                    toUpdate.Type = preference.Type;
                    context.SaveChanges();
                    return true;
                }

                // inserts object
                context.Preferences.Add(preference);
                context.SaveChanges();
                return false;
            }
        }

    }

}
