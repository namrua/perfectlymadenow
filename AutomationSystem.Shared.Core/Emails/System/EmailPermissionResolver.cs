using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Base.Contract.Models;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Shared.Core.Emails.System
{
    public class EmailPermissionResolver : IEmailPermissionResolver
    {
        private readonly IIdentityResolver identityResolver;

        private readonly ConcurrentBag<IEmailPermissionResolverForEntity> permissionResolvers = new ConcurrentBag<IEmailPermissionResolverForEntity>();

        public EmailPermissionResolver(
            IEnumerable<IEmailPermissionResolverForEntity> permissionResolvers,
            IIdentityResolver identityResolver)
        {
            this.identityResolver = identityResolver;

            RegisterEmailPermissionResolvers(permissionResolvers);
        }

        public void CheckEmailIsGranted(EmailEntityId emailEntityId, EmailTypeEnum emailTypeId)
        {
            var resolvers = GetResolvers(emailEntityId.TypeId);
            var isGranted = resolvers.Any(x => x.IsGrantedForEmail(emailEntityId, emailTypeId));
            if (!isGranted)
            {
                throw EntitleAccessDeniedException.New(Entitle.CoreEmailTemplates, identityResolver.GetCurrentIdentity(),
                        $"no email permission resolver grants permission to {emailEntityId}")
                    .AddId(entityTypeId: emailEntityId.TypeId, entityId: emailEntityId.Id);
            }
        }

        public void CheckEmailTemplateIsGranted(EmailTemplateEntityId emailTemplateEntityId, EmailTypeEnum emailTypeId)
        {
            var resolvers = GetResolvers(emailTemplateEntityId.TypeId);
            var isGranted = resolvers.Any(x => x.IsGrantedForEmailTemplate(emailTemplateEntityId, emailTypeId));
            if (!isGranted)
            {
                throw EntitleAccessDeniedException.New(Entitle.CoreEmailTemplates, identityResolver.GetCurrentIdentity(),
                        $"no email permission resolver grants permission to {emailTemplateEntityId}")
                    .AddId(entityTypeId: emailTemplateEntityId.TypeId, entityId: emailTemplateEntityId.Id);
            }
        }

        #region private methods
        
        private void RegisterEmailPermissionResolvers(IEnumerable<IEmailPermissionResolverForEntity> permissionResolversToRegister)
        {
            foreach (var permissionResolver in permissionResolversToRegister)
            {
                permissionResolvers.Add(permissionResolver);
            }
        }

        private List<IEmailPermissionResolverForEntity> GetResolvers(EntityTypeEnum? entityTypeId)
        {
            return permissionResolvers.Where(x => x.SupportedEntityTypeIds.Contains(entityTypeId)).ToList();
        }

        #endregion
    }
}
