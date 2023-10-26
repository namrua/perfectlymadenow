using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Registrations.System.Emails
{
    public class EmailPermissionResolverForRegistration : IEmailPermissionResolverForEntity
    {
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IIdentityResolver identityResolver;

        public List<EntityTypeEnum?> SupportedEntityTypeIds => new List<EntityTypeEnum?> { EntityTypeEnum.MainClassRegistration, EntityTypeEnum.MainClassRegistrationInvitation };

        public EmailPermissionResolverForRegistration(IRegistrationDatabaseLayer registrationDb, IIdentityResolver identityResolver)
        {
            this.registrationDb = registrationDb;
            this.identityResolver = identityResolver;
        }

        public bool IsGrantedForEmail(EmailEntityId emailEntityId, EmailTypeEnum emailTypeId)
        {
            return IsGranted(emailEntityId.TypeId, emailEntityId.Id);
        }

        public bool IsGrantedForEmailTemplate(EmailTemplateEntityId emailTemplateEntityId, EmailTypeEnum emailTypeId)
        {
            return IsGranted(emailTemplateEntityId.TypeId, emailTemplateEntityId.Id);
        }
        
        #region private methods

        private Class GetClassByEntityTypeAndId(EntityTypeEnum? entityTypeId, long? entityId)
        {
            switch (entityTypeId)
            {
                case EntityTypeEnum.MainClassRegistration:
                    var registration = registrationDb.GetClassRegistrationById(entityId ?? 0, ClassRegistrationIncludes.Class);
                    return registration.Class;

                case EntityTypeEnum.MainClassRegistrationInvitation:
                    var invitation = registrationDb.GetClassRegistrationInvitationById(entityId ?? 0, ClassRegistrationInvitationIncludes.Class);
                    return invitation.Class;

                default:
                    return null;
            }
        }
        
        private bool IsGranted(EntityTypeEnum? entityTypeId, long? entityId)
        {
            var cls = GetClassByEntityTypeAndId(entityTypeId, entityId);
            if (cls == null)
            {
                return false;
            }

            var result = identityResolver.IsEntitleGrantedForClass(cls);
            return result;
        }

        #endregion
    }
}
