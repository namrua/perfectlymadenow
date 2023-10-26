using System.Collections.Generic;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation
{
    /// <summary>
    /// Validates email input for email parametes
    /// </summary>
    public interface IEmailParameterValidator
    {

        // validates text for specified validation
        List<string> Validate(EmailInputType type, string text, EmailValidationType validationType);       

    }

}
