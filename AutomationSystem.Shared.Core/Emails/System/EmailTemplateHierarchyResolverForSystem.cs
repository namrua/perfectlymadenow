using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;

namespace AutomationSystem.Shared.Core.Emails.System
{
    public class EmailTemplateHierarchyResolverForSystem : IEmailTemplateHierarchyResolverForEntity
    {
        public EntityTypeEnum? EntityTypeId => null;

        public EmailTemplateEntityHierarchy GetHierarchyForParent(long? entityId)
        {
            return new EmailTemplateEntityHierarchy
            {
                CanUseDefault = true
            };
        }

        public EmailTemplateEntityHierarchy GetHierarchy(long? entityId)
        {
            var result = new EmailTemplateEntityHierarchy
            {
                CanUseDefault = false
            };
            result.Entities.Add(new EmailTemplateEntityId());

            return result;
        }

    }
}
