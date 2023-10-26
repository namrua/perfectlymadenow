using System;

namespace AutomationSystem.Main.Core.Registrations.Data.Models
{

    /// <summary>
    /// Determines class registration invitation includes
    /// </summary>
    [Flags]
    public enum ClassRegistrationInvitationIncludes
    {
        None = 0,
        ClassRegistration = 1 << 0,
        Class = 1 << 1
    }

}
