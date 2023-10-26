using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Email validation info
    /// </summary>
    public class EmailTemplateValidationResult
    {

        // public properties
        public bool IsValid { get; set; }
        public List<string> PublicValidationMessages { get; set; }
        public List<string> ValidationMessages { get; set; }

        // constructor
        public EmailTemplateValidationResult()
        {
            PublicValidationMessages = new List<string>();
            ValidationMessages = new List<string>();
        }

    }

}
