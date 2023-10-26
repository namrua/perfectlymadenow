using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;

namespace AutomationSystem.Main.Core.Profiles.System.Emails
{
    public class EmailPermissionResolverForProfile : IEmailPermissionResolverForEntity
    {
        private readonly IEmailTypeResolver emailTypeResolver;
        private readonly IIdentityResolver identityResolver;

        public List<EntityTypeEnum?> SupportedEntityTypeIds => new List<EntityTypeEnum?> { EntityTypeEnum.MainProfile };

        public EmailPermissionResolverForProfile(IEmailTypeResolver emailTypeResolver, IIdentityResolver identityResolver)
        {
            this.emailTypeResolver = emailTypeResolver;
            this.identityResolver = identityResolver;
        }

        public bool IsGrantedForEmail(EmailEntityId emailEntityId, EmailTypeEnum emailTypeId)
        {
            return IsGranted(emailEntityId.Id, emailTypeId);
        }

        public bool IsGrantedForEmailTemplate(EmailTemplateEntityId emailTemplateEntityId, EmailTypeEnum emailTypeId)
        {
            return IsGranted(emailTemplateEntityId.Id, emailTypeId);
        }

        #region private methods


        private bool IsGranted(long? profileId, EmailTypeEnum emailTypeId)
        {
            var onlyWwa = identityResolver.ResolveOnlyWwaEmailTypes(profileId);
            if (!onlyWwa.HasValue)
            {
                return false;
            }

            var emailTypes = emailTypeResolver.GetEmailTypesForProfile(onlyWwa.Value);
            return emailTypes.Contains(emailTypeId);
        }

        #endregion
    }
}
