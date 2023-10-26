using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Core.Emails.System
{
    public interface IEmailTypeResolver
    {
        EmailTypeEnum ResolveEmailTypeForRegistration(EmailTypeEnum emailTypeId, RegistrationTypeEnum registrationTypeId);

        EmailTypeEnum GetWwaEmailTypeFrom(EmailTypeEnum emailTypeId);

        bool IsWwaEmailType(EmailTypeEnum emailTypeId);

        HashSet<EmailTypeEnum> GetEmailTypesForProfile(bool onlyWwaEmailTypes);
    }
}
