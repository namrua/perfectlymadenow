using System;

namespace AutomationSystem.Main.Core.FormerClasses.Data.Models
{
    /// <summary>
    /// Determines former student includes
    /// </summary>
    [Flags]
    public enum FormerStudentIncludes
    {
        None = 0,
        Address = 1 << 0,
        AddressCountry = 1 << 1,
        FormerClass = 1 << 2,      
        FormerClassClassType = 1 << 3,
        FormerClassProfile = 1 << 4
    }

}
