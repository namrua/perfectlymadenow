using System;

namespace AutomationSystem.Shared.Contract.Emails.Data.Models
{
    /// <summary>
    /// Determines special operation options on email entities
    /// </summary>
    [Flags]
    public enum EmailOperationOption
    {
        None = 0x00,
        OmitSealedCondition = 0x01,             // omits checking of sealed condition
        CheckNoEmails = 0x02,                   // checks no email condition
    }

}
