using System;

namespace AutomationSystem.Main.Core.FormerClasses.Data.Models
{
    /// <summary>
    /// Determines former class includes
    /// </summary>
    [Flags]
    public enum FormerClassIncludes
    {
        None = 0,      
        ClassType = 1 << 0,     
        FormerStudent = 1 << 1,
        FormerStudentAddress = 1 << 2,
        FormerStudentAddressCountry = 1 << 3,
        Profile =  1 << 4
    }

}
