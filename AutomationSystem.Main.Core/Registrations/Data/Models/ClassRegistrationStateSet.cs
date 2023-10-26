namespace AutomationSystem.Main.Core.Registrations.Data.Models
{
    /// <summary>
    /// Class registration state sets
    /// </summary>
    public enum ClassRegistrationStateSet
    {
        All,
        New,                        // new registraton waiting for approval
        NewApproved,                // registrations that are new or approved (not temporary or clanceled)
        Approved,                   // registration is approve and ready for class
        Temporary,                  // temporary
        Canceled                    // canceled
    }

}
