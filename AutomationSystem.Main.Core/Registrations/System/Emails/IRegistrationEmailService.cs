using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Emails.System.Models;

namespace AutomationSystem.Main.Core.Registrations.System.Emails
{
    public interface IRegistrationEmailService
    {
        long SendRegistrationEmailByType(
            EmailTypeEnum emailTypeId,
            long registrationId,
            RecipientType recipientType,
            bool createIncident = false);

        long SendRegistrationEmailTemplate(long emailTemplateId, bool sealTemplate = false);
        
        long SendClassRegistrationInvitationEmail(long invitationId, EmailTypeEnum emailTypeId);
    }
}
