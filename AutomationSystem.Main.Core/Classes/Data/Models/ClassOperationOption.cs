using System;

namespace AutomationSystem.Main.Core.Classes.Data.Models
{
    /// <summary>
    /// Determines special operation options on class entities
    /// </summary>
    [Flags]
    public enum ClassOperationOption
    {
        None = 0,
        KeepOwnerId = 1 << 0,                               // does not change OwnerId      
        CheckOperation = 1 << 1,                            // checks class operation
        ApplyEditRestrictions = 1 << 2,                     // applies editing restriction when class is not allowed to perform full edit 
    }
}