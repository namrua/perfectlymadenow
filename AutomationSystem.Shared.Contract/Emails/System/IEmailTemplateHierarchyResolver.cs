using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System.Models;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface IEmailTemplateHierarchyResolver
    {
        EmailTemplateEntityHierarchy GetHierarchyForParent(EmailTemplateEntityId emailTemplateEntityId);

        EmailTemplateEntityHierarchy GetHierarchy(EmailTemplateEntityId emailTemplateEntityId);
    }
}
