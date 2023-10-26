namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Home service error type
    /// </summary>
    public enum HomeServiceErrorType
    {        
        ClassRegistrationClosed,                // Registration of class is closed
        ClassRegistrationNotStarted,            // Registration of class not started
        RegistrationComplete,                   // Current registration complete
        InvalidRegistrationStep,                // Invalid registration step in the workflow
        RegistrationTypeNotAllowed,             // Registration type is not allowed for the class
        PreRegistrationClosed,                  // Pre-registration is closed
        InvitationExpired,                      // Expired intivation
        InvalidPage,                            // Invalid page - wrong page arguments
        MaterialsNotAvailable,                  // Class materials are not available
        GenericError,                           // Other errors
    }
}