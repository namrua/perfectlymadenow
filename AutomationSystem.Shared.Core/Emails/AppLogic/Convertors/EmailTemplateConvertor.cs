using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.Convertors
{
    /// <summary>
    /// Email template convertor
    /// </summary>
    public class EmailTemplateConvertor : IEmailTemplateConvertor
    {
        
        public readonly IEmailDatabaseLayer emailDb;


        // private properties
        public EmailTemplateConvertor(IEmailDatabaseLayer emailDb)
        {
            this.emailDb = emailDb;
        }


        // converts EmailTemplate to EmailTemplateDetail
        public EmailTemplateDetail ConvertToEmailTemplateDetail(EmailTemplate template, List<EmailParameter> emailParameters = null)
        {
            if (template.EmailType == null)
                throw new InvalidOperationException("EmailType is not included into EmailTemplate object.");
            if (template.EmailTemplateParameters == null)
                throw new InvalidOperationException("EmailTemplateParameters is not included into EmailTemplate object.");
            if (template.Language == null)
                throw new InvalidOperationException("Language is not included into EmailTemplate object.");

            var parameters = emailParameters ?? emailDb.GetEmailParametersByIds(template.EmailTemplateParameters.Select(x => x.EmailParameterId));
            var requiredParams = new HashSet<long>(template.EmailTemplateParameters.Where(x => x.IsRequired).Select(x => x.EmailParameterId));

            // creates email template detail
            var result = new EmailTemplateDetail
            {
                Type = template.EmailType,
                Language = template.Language,               

                EmailTemplateId = template.EmailTemplateId,                
                Subject = template.Subject,
                Body = template.Text,
                FillingNote = template.FillingNote,
                Parameters = parameters.Select(x => ConvertToEmailTemplateParameterDetail(x, requiredParams.Contains(x.EmailParameterId))).ToList(),
                EmailTemplateEntityId = new EmailTemplateEntityId(template.EntityTypeId, template.EntityId)
            };
            return result;
        }

        // converts email template to email template list item
        public EmailTemplateListItem ConvertToEmailTemplateListItem(EmailTemplate template)
        {
            if (template.Language == null)
                throw new InvalidOperationException("Language is not included into EmailTemplate object.");

            var result = new EmailTemplateListItem
            {
                EmailTemplateId = template.EmailTemplateId,
                EmailTypeId = template.EmailTypeId,
                Subject = template.Subject,
                LanguageId = template.LanguageId,
                Language = template.Language.Description,
                IsValid = template.IsValidated
            };
            return result;
        }



        // initialize email template metadata for edit for existing template
        public EmailTemplateMetadataForEdit InitializeEmailTemplateMetadataForEdit(EmailTemplate template, bool fromParentTemplate = false)
        {
            if (template.EmailType == null)
                throw new InvalidOperationException("EmailType is not included into EmailTemplate object.");
            if (template.EmailTemplateParameters == null)
                throw new InvalidOperationException("EmailTemplateParameters is not included into EmailTemplate object.");

            var parameters = emailDb.GetEmailParametersByIds(template.EmailTemplateParameters.Select(x => x.EmailParameterId));

            // assembles result - model
            var result = new EmailTemplateMetadataForEdit
            {
                IsNew = fromParentTemplate,                    // construction from defautl template implicitly expects that etm for edit is new
                Type = template.EmailType,
                Parameters = parameters.Select(x => ConvertToEmailTemplateParameterDetail(x, false)).ToList()
            };

            // resolves whether metadata for edit is constructed from default template (language is omitted)
            if (!fromParentTemplate)
            {
                if (template.Language == null)
                    throw new InvalidOperationException("Language is not included into EmailTemplate object.");

                result.Language = template.Language;
            }

            return result;
        }

        // converts template to metadata template form
        public EmailTemplateMetadataForm ConvertToEmailTemplateMetadataForm(EmailTemplate template,
            bool fromParentTemplate = false)
        {
            var result = new EmailTemplateMetadataForm
            {                
                EmailTypeId = template.EmailTypeId,                
                FillingNote = template.FillingNote,
                RequiredParameters = template.EmailTemplateParameters
                    .Where(x => x.IsRequired).Select(x => x.EmailParameterId).ToArray(),
                EntityTypeId = template.EntityTypeId,
                EntityId = template.EntityId
            };

            // resolves whether metadata form is constructed from default template (language and templateId is omitted)
            if (fromParentTemplate)
            {            
                result.ParentEmailTemplateId = template.EmailTemplateId;
            }
            else
            {
                result.EmailTemplateId = template.EmailTemplateId;
                result.LanguageId = template.LanguageId;
            }            

            return result;
        }

        


        // converts email template metadata form to EmailTemplate
        public EmailTemplate ConvertToEmailTemplate(EmailTemplateMetadataForm form, EmailTemplate defaultTemplate, 
            List<EmailParameter> defaultParams, EmailTemplate template)
        {
            if (defaultTemplate.EmailTemplateParameters == null)
                throw new InvalidOperationException("EmailTemplateParameters is not included into default EmailTemplate object.");

            // creates email template
            var result = new EmailTemplate
            {
                EmailTemplateId = form.EmailTemplateId,
                LanguageId = form.LanguageId,
                EmailTypeId = form.EmailTypeId,
                IsDefault = false,
                IsLocalisable = defaultTemplate.IsLocalisable,
                EntityTypeId = form.EntityTypeId,
                EntityId = form.EntityId,
                Subject = template?.Subject ?? defaultTemplate.Subject,
                Text = template?.Text ?? defaultTemplate.Text,
                FillingNote = form.FillingNote
            };

            // initializes template to save with parameters            
            var requiredParams = new HashSet<long>(form.RequiredParameters ?? new long[0]);
            foreach (var defParameter in defaultParams)
            {
                var param = new EmailTemplateParameter();
                param.EmailParameterId = defParameter.EmailParameterId;
                param.IsRequired = requiredParams.Contains(defParameter.EmailParameterId);
                result.EmailTemplateParameters.Add(param);
            }           

            return result;
        }



        // initialize email template text for edit
        public EmailTemplateTextForEdit InitializeEmailTemplateTextForEdit(EmailTemplate template, List<EmailParameter> emailParameters = null)
        {
            if (template.EmailType == null)
                throw new InvalidOperationException("EmailType is not included into EmailTemplate object.");
            if (template.EmailTemplateParameters == null)
                throw new InvalidOperationException("EmailTemplateParameters is not included into EmailTemplate object.");
            if (template.Language == null)
                throw new InvalidOperationException("Language is not included into EmailTemplate object.");

            // loads parameters
            var parameters = emailParameters ?? emailDb.GetEmailParametersByIds(template.EmailTemplateParameters.Select(x => x.EmailParameterId));
            var requiredParameters = new HashSet<long>(template.EmailTemplateParameters.Where(x => x.IsRequired).Select(x => x.EmailParameterId));

            // assembles result - model
            var result = new EmailTemplateTextForEdit
            {
                IsDisabled = template.IsSealed,
                Type = template.EmailType,
                Language = template.Language,
                FillingNote = template.FillingNote,
                Parameters = parameters.Select(x => ConvertToEmailTemplateParameterDetail(x, requiredParameters.Contains(x.EmailParameterId))).ToList()
            };
            return result;
        }


        // converts template to text template form
        public EmailTemplateTextForm ConvertToEmailTemplateEditForm(EmailTemplate template)
        {           
            var result = new EmailTemplateTextForm
            {
                EmailTemplateId = template.EmailTemplateId,
                Subject = template.Subject,
                Text = template.Text
            };
            return result;
        }


        // converts email template metadata form to EmailTemplate
        public EmailTemplate ConvertToEmailTemplate(EmailTemplateTextForm form)
        {
            var result = new EmailTemplate
            {
                EmailTemplateId = form.EmailTemplateId,
                Subject = form.Subject,
                Text = form.Text,          
            };
            return result;

        }


        // clones email template
        public EmailTemplate CloneEmailTemplate(EmailTemplate template)
        {
            if (template.EmailTemplateParameters == null)
                throw new InvalidOperationException("EmailTemplateParameters is not included into EmailTemplate object.");

            var result = new EmailTemplate
            {
                LanguageId = template.LanguageId,
                EmailTypeId = template.EmailTypeId,
                IsDefault = false,
                IsLocalisable = false,
                EntityTypeId = null,
                EntityId = null,
                Subject = template.Subject,
                Text = template.Text,
                FillingNote = template.FillingNote,
                IsValidated = template.IsValidated,
                IsHtml = template.IsHtml
            };

            foreach (var origParam in template.EmailTemplateParameters)
            {
                var resultParam = new EmailTemplateParameter
                {
                    EmailParameterId = origParam.EmailParameterId,
                    IsRequired = origParam.IsRequired,
                };
                result.EmailTemplateParameters.Add(resultParam);
            }
            return result;
        }


        // reset email template
        public void ResetEmailTemplate(EmailTemplate template, EmailTemplate parentTemplate)
        {
            if (parentTemplate.EmailTemplateParameters == null)
                throw new InvalidOperationException("EmailTemplateParameters is not included into parent EmailTemplate object.");

            template.FillingNote = parentTemplate.FillingNote;
            template.IsValidated = parentTemplate.IsValidated;
            template.Subject = parentTemplate.Subject;
            template.Text = parentTemplate.Text;
            template.EmailTemplateParameters = parentTemplate.EmailTemplateParameters
                .Select(x => new EmailTemplateParameter
                {
                    EmailTemplateId = template.EmailTemplateId,
                    EmailParameterId = x.EmailParameterId,
                    IsRequired = x.IsRequired
                }).ToList();
        }


        #region private methods

        // converts Email parameter to email template parameter detail
        private EmailTemplateParameterDetail ConvertToEmailTemplateParameterDetail(EmailParameter parameter, bool isRequired)
        {
            var result = new EmailTemplateParameterDetail
            {
                EmailParameterId = parameter.EmailParameterId,
                IsRequired = isRequired,
                Name = parameter.Name,
                Description = parameter.Description
            };
            return result;
        }

        #endregion

    }

}
