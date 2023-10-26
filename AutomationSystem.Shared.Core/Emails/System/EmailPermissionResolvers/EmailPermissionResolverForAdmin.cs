using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Shared.Core.Emails.System.EmailPermissionResolvers
{
    /// <summary>
    /// Resolves permissions for email templates and emails for email administrator
    /// </summary>
    public class EmailPermissionResolverForAdmin : IEmailPermissionResolverForEntity
    {
        private readonly IIdentityResolver identityResolver;

        public List<EntityTypeEnum?> SupportedEntityTypeIds => new List<EntityTypeEnum?>{ null, EntityTypeEnum.CoreEmail };

        public EmailPermissionResolverForAdmin(IIdentityResolver identityResolver)
        {
            this.identityResolver = identityResolver;
        }

        public bool IsGrantedForEmail(EmailEntityId emailEntityId, EmailTypeEnum emailTypeId)
        {
            var result = identityResolver.IsEntitleGranted(Entitle.CoreEmailTemplatesIntegration);
            return result;
        }

        public bool IsGrantedForEmailTemplate(EmailTemplateEntityId emailTemplateEntityId, EmailTypeEnum emailTypeId)
        {
            var result = identityResolver.IsEntitleGranted(Entitle.CoreEmailTemplates);
            return result;
        }
    }

}
