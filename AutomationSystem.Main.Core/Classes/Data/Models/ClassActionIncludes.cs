using System;

namespace AutomationSystem.Main.Core.Classes.Data.Models
{
    /// <summary>
    /// Determines class acition includes
    /// </summary>
    [Flags]
    public enum ClassActionIncludes
    {
        None = 0,
        Class = 1 << 0,
        ClassActionType = 1 << 1,
        ClassClassStyle = 1 << 2
    }
}