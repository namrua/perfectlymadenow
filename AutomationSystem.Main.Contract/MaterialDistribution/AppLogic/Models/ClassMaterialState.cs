namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Determines class material lifetime states
    /// </summary>
    public enum ClassMaterialState
    {
        PreparingStage,
        Unlocked,
        Locked,
        LockedByAutolock,
        LockedByEndOfClass,
    }
}
