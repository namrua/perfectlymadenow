using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation;

namespace AutomationSystem.Shared.Core.Emails.AppLogic
{
    /// <summary>
    /// Encapsulates common logic for EmailTemplate administration classes
    /// </summary>
    public interface IEmailTemplateAdministrationCommonLogic
    {

        // validates email template    
        EmailTemplateValidationResult ValidateEmailTemplate(string subject, string text, IEmailParameterValidator paramsValidator);

    }

}
