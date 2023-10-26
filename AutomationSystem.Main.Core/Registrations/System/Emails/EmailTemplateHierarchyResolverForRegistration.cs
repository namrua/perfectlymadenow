using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;

namespace AutomationSystem.Main.Core.Registrations.System.Emails
{
    public class EmailTemplateHierarchyResolverForRegistration : IEmailTemplateHierarchyResolverForEntity
    {
        private readonly IRegistrationDatabaseLayer registrationDb;

        public EmailTemplateHierarchyResolverForRegistration(IRegistrationDatabaseLayer registrationDb)
        {
            this.registrationDb = registrationDb;
        }

        public EntityTypeEnum? EntityTypeId => EntityTypeEnum.MainClassRegistration;

        public EmailTemplateEntityHierarchy GetHierarchyForParent(long? entityId)
        {
            var profileId = GetProfileIdByRegistrationId(entityId ?? 0);

            var result = new EmailTemplateEntityHierarchy
            {
                CanUseDefault = false
            };
            result.Entities.Add(new EmailTemplateEntityId());
            result.Entities.Add(new EmailTemplateEntityId(EntityTypeEnum.MainProfile, profileId));

            return result;
        }

        public EmailTemplateEntityHierarchy GetHierarchy(long? entityId)
        {
            throw new InvalidOperationException("Email template hierarchy level on Registration is not supported.");
        }

        #region private methods

        private long GetProfileIdByRegistrationId(long registrationId)
        {
            var registration = registrationDb.GetClassRegistrationById(registrationId);
            if (registration == null)
            {
                throw new ArgumentException($"There is no registration with id {registrationId}.");
            }

            return registration.ProfileId;
        }

        #endregion
    }
}
