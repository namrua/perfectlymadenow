using System;

namespace AutomationSystem.Shared.Core.Emails.AppLogic.ParameterValidation
{
    /// <summary>
    /// Type of email parameter validation
    /// </summary>
    [Flags]
    public enum EmailValidationType
    {
        Allowed = 1,                // unknown parameters cause validation error
        Required = 2                // missing required parameters cause validation error
    }

}
