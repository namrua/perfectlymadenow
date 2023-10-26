using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.System.Models;

namespace AutomationSystem.Shared.Contract.Emails.System
{
    public interface IEmailTemplateHierarchyResolverForEntity
    {
        EntityTypeEnum? EntityTypeId { get; }

        EmailTemplateEntityHierarchy GetHierarchyForParent(long? entityId);

        EmailTemplateEntityHierarchy GetHierarchy(long? entityId);
    }
}
