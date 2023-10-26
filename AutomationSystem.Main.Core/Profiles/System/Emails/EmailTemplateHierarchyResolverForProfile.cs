using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;

namespace AutomationSystem.Main.Core.Profiles.System.Emails
{
    public class EmailTemplateHierarchyResolverForProfile : IEmailTemplateHierarchyResolverForEntity
    {
        public EntityTypeEnum? EntityTypeId => EntityTypeEnum.MainProfile;

        public EmailTemplateEntityHierarchy GetHierarchyForParent(long? entityId)
        {
            var result = new EmailTemplateEntityHierarchy
            {
                CanUseDefault = true
            };
            result.Entities.Add(new EmailTemplateEntityId());

            return result;
        }

        public EmailTemplateEntityHierarchy GetHierarchy(long? entityId)
        {
            var result = new EmailTemplateEntityHierarchy
            {
                CanUseDefault = false
            };
            result.Entities.Add(new EmailTemplateEntityId());
            result.Entities.Add(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, entityId));

            return result;
        }

    }
}
