using System.Collections.Generic;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation
{
    /// <summary>
    /// Validation factory creates email parameter validator
    /// </summary>
    public interface IEmailParameterValidatorFactory
    {

        // gets validation by email template and parameters
        IEmailParameterValidator GetValidatorByEmailTemplate(EmailTemplate template, IEnumerable<EmailParameter> parameters);

        // gets validation by email template params
        IEmailParameterValidator GetValidationByEmailTemplateParameters(IEnumerable<EmailTemplateParameter> templateParameters);
      
    }

}
