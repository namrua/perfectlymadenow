using AutomationSystem.Main.Core.Preferences.System.Models;
using AutomationSystem.Shared.Contract.Preferences.Data;

namespace AutomationSystem.Main.Core.Preferences.System
{



    /// <summary>
    /// Provides saved preferences
    /// </summary>
    public class MainPreferenceProvider : IMainPreferenceProvider
    {

        // private components
        private readonly IPreferenceDatabaseLayer preferenceDb;

        // constructor
        public MainPreferenceProvider(IPreferenceDatabaseLayer preferenceDb)
        {
            this.preferenceDb = preferenceDb;
        }


        // gets helpdesk email address
        public string GetMasterCoordinatorEmail()
        {
            var result = preferenceDb.GetPreferenceByKey(MainPreferenceKey.EmailMasterCoordinatorRecipient).Value;
            return result;
        }

        // gets Master Lead Instructor id
        public long? GetMasterLeadInstructorId()
        {
            var value = preferenceDb.GetPreferenceByKey(MainPreferenceKey.PersonMasterLeadInstructor).Value;
            var result = GetNullableLong(value);
            return result;
        }


        #region

        // gets long? from string
        public long? GetNullableLong(string value)
        {
            if (long.TryParse(value, out var result))
                return result;
            return null;
        }

        #endregion

    }

}
