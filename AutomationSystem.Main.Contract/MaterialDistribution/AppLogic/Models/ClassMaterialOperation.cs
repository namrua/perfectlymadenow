namespace AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models
{
    /// <summary>
    /// Determines class material operations
    /// </summary>
    public enum ClassMaterialOperation
    {
        Unlock,
        SendNotification,
        Lock,
        InitializeMaterialRecipient,
        UnlockRecipient,
        LockRecipient,
        SendNotificationToRecipient
    }
}
