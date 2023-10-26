using System;

namespace AutomationSystem.Main.Core.FormerClasses.Data.Models
{
    /// <summary>
    /// Determines special operation options on former entities
    /// </summary>
    [Flags]
    public enum FormerOperationOption
    {
        None = 0,    
        KeepOwnerId = 1 << 0,                             // does not change OwnerId
        CheckStudentNotUsedForReview = 1 << 2,            // checks whether student is used for review (and cannot be deleted)
        CheckStudentClassConsistency = 1 << 3,            // checks whether students's class was not tempered in the form
    }   

}
