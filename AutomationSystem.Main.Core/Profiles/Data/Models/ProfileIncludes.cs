using System;

namespace AutomationSystem.Main.Core.Profiles.Data.Models
{
    /// <summary>
    /// Determines profile includes
    /// </summary>
    [Flags]
    public enum ProfileIncludes
    {
        None = 0,
        ClassPreference = 1 << 0,
        ClassPreferenceClassPreferenceExpenses = 1 << 1,
        ProfileUsers = 1 << 2,
        ClassPreferenceLocationInfo = 1 << 3,
        ClassPreferenceCurrency = 1 << 4
    }
}
