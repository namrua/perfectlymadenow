using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.System.Emails;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Main.Core.Registrations.System.Emails
{
    public class RegistrationEmailService : IRegistrationEmailService
    {
        private readonly IEmailServiceHelper helper;
        private readonly IEmailDatabaseLayer emailDb;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IIncidentLogger incidentLogger;
        private readonly IEmailTextResolverFactory emailTextResolverFactory;
        private readonly IEmailTemplateResolver emailTemplateResolver;
        private readonly IClassEmailParameterResolverFactory classEmailParameterResolverFactory;
        private readonly IRegistrationEmailParameterResolverFactory registrationEmailParameterResolverFactory;
        private readonly IMainEmailHelper mainHelper;

        // constructor
        public RegistrationEmailService(
            IEmailServiceHelper helper,
            IEmailDatabaseLayer emailDb,
            IRegistrationDatabaseLayer registrationDb,
            IIncidentLogger incidentLogger,
            IEmailTextResolverFactory emailTextResolverFactory,
            IEmailTemplateResolver emailTemplateResolver,
            IClassEmailParameterResolverFactory classEmailParameterResolverFactory,
            IRegistrationEmailParameterResolverFactory registrationEmailParameterResolverFactory,
            IMainEmailHelper mainHelper)
        {
            this.helper = helper;
            this.emailDb = emailDb;
            this.registrationDb = registrationDb;
            this.incidentLogger = incidentLogger;
            this.emailTextResolverFactory = emailTextResolverFactory;
            this.emailTemplateResolver = emailTemplateResolver;
            this.classEmailParameterResolverFactory = classEmailParameterResolverFactory;
            this.registrationEmailParameterResolverFactory = registrationEmailParameterResolverFactory;
            this.mainHelper = mainHelper;
        }

        public long SendRegistrationEmailByType(
           EmailTypeEnum emailTypeId,
           long registrationId,
           RecipientType recipientType,
           bool createIncident = false)
        {
            var result = 0L;
            try
            {
                // loads registration
                var registration = registrationDb.GetClassRegistrationById(registrationId,
                    ClassRegistrationIncludes.Class | ClassRegistrationIncludes.Addresses | ClassRegistrationIncludes.ClassRegistrationLastClass);
                if (registration == null)
                {
                    throw new ArgumentException($"There is no Class registration with id {registrationId}.");
                }

                // loads template
                var languageId = GetRegistrationRecipientLanguage(registration, recipientType);
                var template = emailTemplateResolver.GetValidTemplate(
                    emailTypeId,
                    languageId,
                    new EmailTemplateEntityId(EntityTypeEnum.MainProfile, registration.ProfileId));

                // assembles text resolver
                var paramResolver = registrationEmailParameterResolverFactory.CreateRegistrationParameterResolver(registration);
                var textResolver = emailTextResolverFactory.CreateEmailTextResolver(paramResolver);
                var recipient = mainHelper.GetRegistrationRecipientEmail(registration, recipientType);

                // gets sender
                var sender = mainHelper.GetSenderInfoByPersonId(registration.Class.CoordinatorId);

                // sends email
                result = helper.SendEmailForTemplate(template, textResolver,
                    new EmailEntityId(EntityTypeEnum.MainClassRegistration, registrationId),
                    recipient, sender, (int)SeverityEnum.High);
            }
            catch (Exception e)
            {
                if (!createIncident) throw;
                incidentLogger.LogIncident(IncidentForLog.New(IncidentTypeEnum.EmailError, e).Entity(EntityTypeEnum.MainClassRegistration, registrationId));
            }
            return result;
        }

        public long SendRegistrationEmailTemplate(long emailTemplateId, bool sealTemplate = false)
        {
            // loads template
            var template = emailTemplateResolver.GetEmailTemplateById(emailTemplateId);
            if (template.EntityTypeId != EntityTypeEnum.MainClassRegistration || !template.EntityId.HasValue)
            {
                throw new ArgumentException($"Email template {emailTemplateId} has inconsistent entity relation {template.EmailTemplateId} {template.EntityId}");
            }

            // loads registration
            var registrationId = template.EntityId.Value;
            var registration = registrationDb.GetClassRegistrationById(registrationId,
                ClassRegistrationIncludes.Class | ClassRegistrationIncludes.Addresses | ClassRegistrationIncludes.ClassRegistrationLastClass);
            if (registration == null)
            {
                throw new ArgumentException($"There is no Class registration with id {registrationId}.");
            }

            // assembles text resolver
            var paramResolver = registrationEmailParameterResolverFactory.CreateRegistrationParameterResolver(registration);
            var textResolver = emailTextResolverFactory.CreateEmailTextResolver(paramResolver);

            // gets sender
            var sender = mainHelper.GetSenderInfoByPersonId(registration.Class.CoordinatorId);

            // sends email
            var result = helper.SendEmailForTemplate(template, textResolver,
                new EmailEntityId(EntityTypeEnum.MainClassRegistration, registrationId),
                mainHelper.GetRegistrationRecipientEmail(registration, RecipientType.Student), sender, (int)SeverityEnum.High);

            // seals template
            if (sealTemplate)
            {
                emailDb.SetEmailTemplatesToSealed(new[] { emailTemplateId });
            }

            return result;
        }

        public long SendClassRegistrationInvitationEmail(long invitationId, EmailTypeEnum emailTypeId)
        {
            // loads invitation with classID
            var invitation = registrationDb.GetClassRegistrationInvitationById(invitationId, ClassRegistrationInvitationIncludes.Class);
            if (invitation == null)
            {
                throw new ArgumentException($"There is no Class registration invitation with id {invitationId}.");
            }

            // loads template
            var template = emailTemplateResolver.GetValidTemplate(
                emailTypeId,
                invitation.LanguageId,
                new EmailTemplateEntityId(EntityTypeEnum.MainProfile, invitation.Class.ProfileId));

            // assembles text resolver
            var classResolver = classEmailParameterResolverFactory.CreateClassParameterResolver(invitation.Class);
            var invitationResolver = registrationEmailParameterResolverFactory.CreateRegistrationInvitationParameterResolver();
            var textResolver = emailTextResolverFactory.CreateEmailTextResolver(invitationResolver, classResolver);
            invitationResolver.Bind(invitation);

            // gets sender
            var sender = mainHelper.GetSenderInfoByPersonId(invitation.Class.CoordinatorId);

            // sends email
            var result = helper.SendEmailForTemplate(template, textResolver,
                new EmailEntityId(EntityTypeEnum.MainClassRegistrationInvitation, invitationId),
                invitation.Email, sender, (int)SeverityEnum.High);

            return result;
        }

        #region private methods

        private LanguageEnum GetRegistrationRecipientLanguage(ClassRegistration registration, RecipientType type)
        {
            var result = LocalisationInfo.DefaultLanguage;
            switch (type)
            {
                // normal student
                case RecipientType.Student:
                    result = registration.LanguageId;
                    break;
            }
            return result;
        }

        #endregion
    }
}
