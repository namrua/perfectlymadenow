using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.Convertors
{
    /// <summary>
    /// Email template convertor
    /// </summary>
    public interface IEmailTemplateConvertor
    {

        // converts EmailTemplate to EmailTemplateDetail
        EmailTemplateDetail ConvertToEmailTemplateDetail(EmailTemplate template, List<EmailParameter> emailParameters = null);

        // converts email template to email template list item
        EmailTemplateListItem ConvertToEmailTemplateListItem(EmailTemplate template);


        // initialize email template metadata for edit 
        EmailTemplateMetadataForEdit InitializeEmailTemplateMetadataForEdit(EmailTemplate template, bool fromParentTemplate = false);

        // converts template to metadata template form
        EmailTemplateMetadataForm ConvertToEmailTemplateMetadataForm(EmailTemplate template, bool fromParentTemplate = false);

        // converts email template metadata form to EmailTemplate
        EmailTemplate ConvertToEmailTemplate(EmailTemplateMetadataForm form, EmailTemplate defaultTemplate,
            List<EmailParameter> defaultParams, EmailTemplate template);


        // initialize email template text for edit
        EmailTemplateTextForEdit InitializeEmailTemplateTextForEdit(EmailTemplate template, List<EmailParameter> emailParameters = null);

        // converts template to text template form
        EmailTemplateTextForm ConvertToEmailTemplateEditForm(EmailTemplate template);

        // converts email template metadata form to EmailTemplate
        EmailTemplate ConvertToEmailTemplate(EmailTemplateTextForm form);

        
        // clones email template
        EmailTemplate CloneEmailTemplate(EmailTemplate template);

        // reset email template
        void ResetEmailTemplate(EmailTemplate template, EmailTemplate parentTemplate);

    }
}
