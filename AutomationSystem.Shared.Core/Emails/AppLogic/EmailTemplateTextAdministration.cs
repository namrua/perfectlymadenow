using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Core.Emails.AppLogic.Convertors;
using AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation;
using AutomationSystem.Shared.Model;
using System;
using System.Linq;

namespace AutomationSystem.Shared.Core.Emails.AppLogic
{
    /// <summary>
    /// Provides email template text administration 
    /// The service is intended for integration with other web pages and contains integration with IDM
    /// </summary>
    public class EmailTemplateTextAdministration : IEmailTemplateTextAdministration
    {

        private readonly IEmailDatabaseLayer emailDb;
        private readonly ICoreEmailService coreEmailService;
        private readonly IEmailConvertor emailConvertor;
        private readonly IEmailParameterValidatorFactory paramsValidatorFactory;
        private readonly IEmailTemplateConvertor templateConvertor;
        private readonly IEmailTemplateAdministrationCommonLogic commonLogic;
        private readonly IEmailTemplateResolver templateResolver;
        private readonly IEmailPermissionResolver emailPermissionResolver;

        // constructor
        public EmailTemplateTextAdministration(
            IEmailDatabaseLayer emailDb,
            ICoreEmailService coreEmailService,
            IEmailTemplateConvertor templateConvertor,
            IEmailConvertor emailConvertor,
            IEmailTemplateAdministrationCommonLogic commonLogic,
            IEmailParameterValidatorFactory paramsValidatorFactory,
            IEmailTemplateResolver templateResolver,
            IEmailPermissionResolver emailPermissionResolver)
        {
            this.emailDb = emailDb;
            this.coreEmailService = coreEmailService;
            this.paramsValidatorFactory = paramsValidatorFactory;
            this.templateConvertor = templateConvertor;
            this.emailConvertor = emailConvertor;
            this.commonLogic = commonLogic;
            this.templateResolver = templateResolver;
            this.emailPermissionResolver = emailPermissionResolver;
        }

        #region email template text editing

        // get text for editation of existing template
        public EmailTemplateTextForEdit GetEmailTemplateTextForEditById(long emailTemplateId)
        {
            // gets template and initializes and validates text for edit
            var template = GetEmailTemplateById(emailTemplateId,
                EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType | EmailTemplateIncludes.Language);
            emailPermissionResolver.CheckEmailTemplateIsGranted(new EmailTemplateEntityId(template.EntityTypeId, template.EntityId), template.EmailTypeId);
            var result = InitializeAndValidateTextForEdit(template);
            return result;
        }


        // gets reset email template text for edit
        public EmailTemplateTextForEdit GetResetEmailTemplateTextForEdit(long emailTemplateId)
        {
            // gets template
            var template = GetEmailTemplateById(emailTemplateId,
                EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType | EmailTemplateIncludes.Language);
            emailPermissionResolver.CheckEmailTemplateIsGranted(new EmailTemplateEntityId(template.EntityTypeId, template.EntityId), template.EmailTypeId);

            // loads parrent subject and text
            var parentTemplate = templateResolver.GetParentTemplate(template);
            template.Subject = parentTemplate.Subject;
            template.Text = parentTemplate.Text;

            // initializes and validates text for edit
            var result = InitializeAndValidateTextForEdit(template);
            return result;
        }


        // get email text for editation by form
        public EmailTemplateTextForEdit GetEmailTemplateTextForEditByForm(EmailTemplateTextForm form, EmailTemplateValidationResult validation)
        {
            // gets template 
            var template = GetEmailTemplateById(form.EmailTemplateId,
                EmailTemplateIncludes.EmailTemplateParameter | EmailTemplateIncludes.EmailType | EmailTemplateIncludes.Language);
            emailPermissionResolver.CheckEmailTemplateIsGranted(new EmailTemplateEntityId(template.EntityTypeId, template.EntityId), template.EmailTypeId);

            // loads email template
            var result = templateConvertor.InitializeEmailTemplateTextForEdit(template);
            result.Form = form;
            result.ValidationResult = validation;
            return result;
        }


        // validates email text
        public EmailTemplateValidationResult ValidateEmailTemplateText(EmailTemplateTextForm form)
        {
            // to IDM needed

            // executes validation
            var templateParams = emailDb.GetEmailTemplateParametersByEmailTemplateId(form.EmailTemplateId);
            var paramsValidator = paramsValidatorFactory.GetValidationByEmailTemplateParameters(templateParams);
            var result = commonLogic.ValidateEmailTemplate(form.Subject, form.Text, paramsValidator);
            return result;
        }


        // saves email template text, return true whether email template is valid
        public void SaveEmailTemplateText(EmailTemplateTextForm form, bool isValidated)
        {
            var toCheck = GetEmailTemplateById(form.EmailTemplateId);
            emailPermissionResolver.CheckEmailTemplateIsGranted(new EmailTemplateEntityId(toCheck.EntityTypeId, toCheck.EntityId), toCheck.EmailTypeId);

            // updates data
            var dbTemplate = templateConvertor.ConvertToEmailTemplate(form);
            dbTemplate.IsValidated = isValidated;
            emailDb.UpdateEmailTemplateText(dbTemplate);
        }

        #endregion

        #region email 

        // gets email detail
        public EmailDetail GetEmailDetail(long emailId)
        {
            var email = emailDb.GetEmailForDetailById(emailId, EmailIncludes.EmailTemplate | EmailIncludes.EntityType);
            if (email == null || !email.EntityTypeId.HasValue || !email.EntityId.HasValue)
            {
                throw new ArgumentException($"There is no Email with id {emailId}");
            }
            emailPermissionResolver.CheckEmailIsGranted(new EmailEntityId(email.EntityTypeId.Value, email.EntityId.Value), email.EmailTemplate.EmailTypeId);

            // assembles result
            var result = emailConvertor.ConvertToEmailDetail(email);
            return result;
        }


        // sends generic test email, returns email id
        public long SendTestEmail(EmailTestSendInfo info, bool allowInvalidTemplate = false)
        {
            var template = emailDb.GetEmailTemplateById(info.EmailTemplateId);
            emailPermissionResolver.CheckEmailIsGranted(info.EmailEntityId, template.EmailTypeId);
            var result = coreEmailService.SendTestEmail(info, allowInvalidTemplate);
            return result;
        }

        #endregion

        #region private methods

        // gets email template by id and tests it is exist
        private EmailTemplate GetEmailTemplateById(long emailTemplateId, EmailTemplateIncludes includes = EmailTemplateIncludes.None)
        {
            // gets default template
            var result = emailDb.GetEmailTemplateById(emailTemplateId, includes);
            if (result == null)
                throw new ArgumentException($"There is no Email template with id {emailTemplateId}.");
            return result;
        }

        // initializes and validates text for edit
        private EmailTemplateTextForEdit InitializeAndValidateTextForEdit(EmailTemplate template)
        {
            // loads parameters
            var parameters = emailDb.GetEmailParametersByIds(template.EmailTemplateParameters.Select(x => x.EmailParameterId));
            var paramsValidator = paramsValidatorFactory.GetValidatorByEmailTemplate(template, parameters);

            // creates and assembles template text for edit
            var result = templateConvertor.InitializeEmailTemplateTextForEdit(template, parameters);
            result.Form = templateConvertor.ConvertToEmailTemplateEditForm(template);
            result.ValidationResult = commonLogic.ValidateEmailTemplate(result.Form.Subject, result.Form.Text, paramsValidator);
            return result;
        }
        #endregion

    }

}
