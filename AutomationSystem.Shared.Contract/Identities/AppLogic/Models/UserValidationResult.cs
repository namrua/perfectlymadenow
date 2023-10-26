namespace AutomationSystem.Shared.Contract.Identities.AppLogic.Models
{
    /// <summary>
    /// Encapsulates user validation informations
    /// </summary>
    public class UserValidationResult
    {

        // public properties
        public bool IsValid { get; set; }
        public bool HasDuplicitName { get; set; }
        public bool HasDuplicitGoogleAccount { get; set; }

    }

}
