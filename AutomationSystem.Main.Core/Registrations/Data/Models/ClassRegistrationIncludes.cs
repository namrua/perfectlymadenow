using System;

namespace AutomationSystem.Main.Core.Registrations.Data.Models
{
    /// <summary>
    /// Determines class registration includes
    /// </summary>
    [Flags]
    public enum ClassRegistrationIncludes
    {
        None = 0,
        ApprovementType = 1 << 0,
        RegistrationType = 1 << 1,
        Class = 1 << 2,
        ClassCurrency = 1 << 3,
        ClassRegistrationPayment = 1 << 4,
        ClassRegistrationInvitations = 1 << 5,       
        Addresses = 1 << 6,
        AddressesCountry = 1 << 7,       
        ClassRegistrationLastClass = 1 << 8,
        ClassRegistrationFiles = 1 << 9,
        ClassClassStyle = 1 << 10,
        ClassProfile = 1 << 11
    }

}
