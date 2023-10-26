using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System.Models;

namespace AutomationSystem.Main.Core.Emails.System
{
    public interface IMainEmailHelper
    {
        SenderInfo GetSenderInfoByPersonId(long? personId);

        string GetRegistrationRecipientEmail(ClassRegistration registration, RecipientType type);
    }
}
