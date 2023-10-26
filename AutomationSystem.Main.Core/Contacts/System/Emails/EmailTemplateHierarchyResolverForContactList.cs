using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using System;

namespace AutomationSystem.Main.Core.Contacts.System.Emails
{
    public class EmailTemplateHierarchyResolverForContactList : IEmailTemplateHierarchyResolverForEntity
    {
        public EntityTypeEnum? EntityTypeId => EntityTypeEnum.MainContactList;

        public EmailTemplateEntityHierarchy GetHierarchy(long? entityId)
        {
            throw new InvalidOperationException("Email template hierarchy level on ContactList is not supported.");
        }

        public EmailTemplateEntityHierarchy GetHierarchyForParent(long? entityId)
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
