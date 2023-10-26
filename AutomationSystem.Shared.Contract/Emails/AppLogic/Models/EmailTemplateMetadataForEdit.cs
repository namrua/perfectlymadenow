using System.Collections.Generic;
using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic.Models
{
    /// <summary>
    /// Email template metadata for edit
    /// </summary>
    public class EmailTemplateMetadataForEdit
    {

        // public properties
        public bool IsNew { get; set; }
        public EmailType Type { get; set; }
        public IEnumItem Language { get; set; }     
        
        [DisplayName("Email parameters")]
        public List<EmailTemplateParameterDetail> Parameters { get; set; }
        public EmailTemplateMetadataForm Form { get; set; }


        // constructors
        public EmailTemplateMetadataForEdit()
        {                   
            Parameters = new List<EmailTemplateParameterDetail>();
            Form = new EmailTemplateMetadataForm();
        }

    }

}
