using System;

namespace AutomationSystem.Main.Core.Persons.Data.Models
{
    /// <summary>
    /// Determines person's includes
    /// </summary>
    [Flags]
    public enum PersonIncludes
    {
        None = 0x00,
        Address = 0x01,
        AddressCountry = 0x02,
        PersonRoles = 0x04,
        Profile = 0x08
    }
    
}
