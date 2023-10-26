using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Email template metadata for edit
    /// </summary>
    public class EmailTemplateTextForEdit
    {

        // public properties       
        public bool IsDisabled { get; set; }
        public EmailType Type { get; set; }
        public IEnumItem Language { get; set; }

        [DisplayName("Instruction note to fill an email")]
        public string FillingNote { get; set; }


        [DisplayName("Email parameters")]
        public List<EmailTemplateParameterDetail> Parameters { get; set; }
        public EmailTemplateTextForm Form { get; set; }
        public EmailTemplateValidationResult ValidationResult { get; set; }
        public EmailTemplateTextContext Context { get; set; }


        // constructors
        public EmailTemplateTextForEdit()
        {
            Form = new EmailTemplateTextForm();
            Parameters = new List<EmailTemplateParameterDetail>();
            Context = new EmailTemplateTextContext();
        }

    }    

}
