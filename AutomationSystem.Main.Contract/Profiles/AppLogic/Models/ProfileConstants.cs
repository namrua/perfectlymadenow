namespace AutomationSystem.Main.Contract.Profiles.AppLogic.Models
{
    /// <summary>
    /// Profile constants
    /// </summary>
    public static class ProfileConstants
    {

        public const string DefaultMoniker = "AlaskaOnline";
        public const string ProfileHomepage = "/Home/Index/{0}";                     // param is profile moniker
        public const string ProfileDistanceHomepage = "/Home/Distance/{0}";          // param is profile moniker

        // constant for filtering
        public const long WithoutProfileId = -1;

    }


}
