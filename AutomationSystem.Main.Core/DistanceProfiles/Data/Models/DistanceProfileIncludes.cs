using System;

namespace AutomationSystem.Main.Core.DistanceProfiles.Data.Models
{
    /// <summary>
    /// Determines distance profile includes
    /// </summary>
    [Flags]
    public enum DistanceProfileIncludes
    {
        None = 0,
        Profile = 1 << 0,
        PriceList = 1 << 1,
        DistanceCoordinator = 1 << 2,
        DistanceCoordinatorAddress = 1 << 3,
        ProfileClassPreference = 1 << 4
    }
}
