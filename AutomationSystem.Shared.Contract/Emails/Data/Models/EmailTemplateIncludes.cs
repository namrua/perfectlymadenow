using System;

namespace AutomationSystem.Shared.Contract.Emails.Data.Models
{
    /// <summary>
    /// Determines email template includes
    /// </summary>
    [Flags]
    public enum EmailTemplateIncludes
    {
        None = 0x00,
        Language = 0x01,
        EmailType = 0x02,
        EmailTemplateParameter = 0x04
    }

}
