using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic
{
    public interface IEmailPermissionResolver
    {
        void CheckEmailTemplateIsGranted(EmailTemplateEntityId emailTemplateEntityId, EmailTypeEnum emailTypeId);

        void CheckEmailIsGranted(EmailEntityId emailEntityId, EmailTypeEnum emailTypeId);
    }
}
