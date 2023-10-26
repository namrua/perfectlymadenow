namespace AutomationSystem.Main.Web.Helpers.Validations.Models
{
    /// <summary>
    /// Encapsulates pdf validation result
    /// </summary>
    public class PdfValidationResult
    {

        public bool IsValid { get; set; } = true;
        public string ValidationMessage { get; set; }

    }

}
