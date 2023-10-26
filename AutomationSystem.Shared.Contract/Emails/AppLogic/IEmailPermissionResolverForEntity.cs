using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Shared.Contract.Emails.AppLogic
{
    /// <summary>
    /// Resolves permissions for email templates and emails
    /// </summary>
    public interface IEmailPermissionResolverForEntity
    {
        bool IsGrantedForEmailTemplate(EmailTemplateEntityId emailTemplateEntityId, EmailTypeEnum emailTypeId);

        bool IsGrantedForEmail(EmailEntityId emailEntityId, EmailTypeEnum emailTypeId);

        List<EntityTypeEnum?> SupportedEntityTypeIds { get; }
    }

}
