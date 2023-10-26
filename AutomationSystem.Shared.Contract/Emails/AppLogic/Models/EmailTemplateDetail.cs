using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Shared.Model;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Encapsulates whole email template detail
    /// </summary>
    public class EmailTemplateDetail
    {

        // public properties        
        public EmailType Type { get; set; }
        public Language Language { get; set; }
        public EmailTemplateValidationResult ValidationResult { get; set; }

        [DisplayName("ID")]
        public long EmailTemplateId { get; set; }

        [DisplayName("Default template ID")]
        public long DefaultEmailTemplateId { get; set; }

        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Body")]
        [TextInputOptions(ControlType = TextControlType.AceTextInput)]
        public string Body { get; set; }

        [DisplayName("Instruction note to fill an email")]
        public string FillingNote { get; set; }

        [DisplayName("Email parameters")]
        public List<EmailTemplateParameterDetail> Parameters { get; set; }

        public EmailTemplateEntityId EmailTemplateEntityId { get; set; }

        // constructor
        public EmailTemplateDetail()
        {
            Parameters = new List<EmailTemplateParameterDetail>();
            ValidationResult = new EmailTemplateValidationResult();
        }

    }
}
