using System;

namespace AutomationSystem.Main.Core.Registrations.Data.Models
{
    /// <summary>
    /// Determines special operation options on class registration entities
    /// </summary>
    [Flags]
    public enum RegistrationOperationOption
    {
        None = 0,
        CheckOperation = 1 << 0,                              // checks registration operation      
        CheckFormerStudent = 1 << 1,                           // checks whether former student id is acceptable as reviewed student
        OnlyForTemporary = 1 << 2                            // checks that registration is temporary
    }

}
