using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using System;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Shared.Contract.Emails.AppLogic;

namespace AutomationSystem.Main.Core.Profiles.AppLogic
{
    public class ProfileEmailAdministration : IProfileEmailAdministration
    {
        private readonly IIdentityResolver identityResolver;
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IEmailTemplateAdministration emailTemplateAdministration;
        private readonly IEmailTypeResolver emailTypeResolver;

        public ProfileEmailAdministration(
            IIdentityResolver identityResolver,
            IProfileDatabaseLayer profileDb,
            IEmailTemplateAdministration emailTemplateAdministration,
            IEmailTypeResolver emailTypeResolver)
        {
            this.identityResolver = identityResolver;
            this.profileDb = profileDb;
            this.emailTemplateAdministration = emailTemplateAdministration;
            this.emailTypeResolver = emailTypeResolver;
        }

        public EmailTypeSummary GetEmailTypeSummaryByProfileId(long profileId)
        {
            var profile = GetProfileById(profileId);
            identityResolver.CheckEntitleForProfile(Entitle.MainProfiles, profile);
            var onlyWwa = identityResolver.ResolveOnlyWwaEmailTypes(profileId);
            var allowedEmailTypes = onlyWwa.HasValue ? emailTypeResolver.GetEmailTypesForProfile(onlyWwa.Value) : new HashSet<EmailTypeEnum>();
            var emailTypeSummary = emailTemplateAdministration.GetEmailTypeSummary(
                    new EmailTemplateEntityId(EntityTypeEnum.MainProfile, profileId),
                    allowedEmailTypes);
            return emailTypeSummary;
        }


        #region private methods

        // gets profile, checks whether profile exists
        private Profile GetProfileById(long profileId, ProfileIncludes includes = ProfileIncludes.None)
        {
            var result = profileDb.GetProfileById(profileId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Profile with id {profileId}.");
            }

            return result;
        }

        #endregion
    }
}
