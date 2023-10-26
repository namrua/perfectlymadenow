using AutomationSystem.Base.Contract.Identities.Models;

namespace AutomationSystem.Shared.Core.Identities.System.Model
{
    /// <summary>
    /// Determines access level to user group including defaults
    /// </summary>
    public class UserGroupAccessLevelWithDefaults
    {

        public UserGroupAccessLevel UserGroupAccessLevel { get; }
        public bool IncludeDefaultUserGroup { get; }

        // constructor
        public UserGroupAccessLevelWithDefaults(UserGroupAccessLevel ugAccessLevel, bool includeDefaultUg)
        {
            UserGroupAccessLevel = ugAccessLevel;
            IncludeDefaultUserGroup = includeDefaultUg;
        }

    }

}
