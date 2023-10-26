namespace AutomationSystem.Main.Core.Preferences.System
{
    /// <summary>
    /// Provides save preferences
    /// </summary>
    public interface IMainPreferenceProvider
    {

        // gets master coordinator emial address
        string GetMasterCoordinatorEmail();

        // gets Master Lead Instructor id
        long? GetMasterLeadInstructorId();

    }

}
