namespace AutomationSystem.Main.Contract.Profiles.AppLogic.Models
{
    /// <summary>
    /// Profile validation result
    /// </summary>
    public class ProfileValidationResult
    {

        public bool IsValid { get; set; } = true;
        public string ForbiddenMoniker { get; set; }

    }


}
