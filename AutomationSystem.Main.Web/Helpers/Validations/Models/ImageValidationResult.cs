namespace AutomationSystem.Main.Web.Helpers.Validations.Models
{
    /// <summary>
    /// Encapsulates image validation result
    /// </summary>
    public class ImageValidationResult
    {

        public bool IsValid { get; set; } = true;
        public string ValidationMessage { get; set; }

    }

}
