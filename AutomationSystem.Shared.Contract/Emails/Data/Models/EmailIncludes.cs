using System;

namespace AutomationSystem.Shared.Contract.Emails.Data.Models
{
    /// <summary>
    /// Determines email includes
    /// </summary>
    [Flags]
    public enum EmailIncludes
    {
        None = 0,
        EmailTemplate = 1 << 0,
        EmailTemplateEmailType = 1 << 1,
        EntityType = 1 << 2
    }

}
