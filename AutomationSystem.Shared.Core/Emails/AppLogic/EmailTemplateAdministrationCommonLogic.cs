using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation;
using AutomationSystem.Shared.Core.HtmlValidation.System;

namespace AutomationSystem.Shared.Core.Emails.AppLogic
{
    /// <summary>
    /// Encapsulates common logic for EmailTemplate administration classes
    /// </summary>
    public class EmailTemplateAdministrationCommonLogic : IEmailTemplateAdministrationCommonLogic
    {

        private readonly IEmailDatabaseLayer emailDb;

        private readonly IHtmlValidatorFactory htmlValidatorFactory;


        // constructor
        public EmailTemplateAdministrationCommonLogic(IEmailDatabaseLayer emailDb)
        {
            this.emailDb = emailDb;

            htmlValidatorFactory = new HtmlValidatorFactory();
        }


        // validates email template    
        public EmailTemplateValidationResult ValidateEmailTemplate(string subject, string text, IEmailParameterValidator paramsValidator)
        {
            // validates html
            var result = new EmailTemplateValidationResult();
            var htmlValidator = htmlValidatorFactory.GetEmailTemplateValidator();
            var bodyHtmlResult = htmlValidator.Validate(text, true);

            result.PublicValidationMessages.AddRange(bodyHtmlResult);
            result.ValidationMessages.AddRange(bodyHtmlResult);

            // validates parameters           
            result.ValidationMessages.AddRange(paramsValidator.Validate(EmailInputType.Subject, subject, EmailValidationType.Allowed));
            result.ValidationMessages.AddRange(paramsValidator.Validate(EmailInputType.Body, text, EmailValidationType.Allowed | EmailValidationType.Required));

            // returns results
            result.IsValid = result.ValidationMessages.Count == 0;
            return result;
        }
    }

}
