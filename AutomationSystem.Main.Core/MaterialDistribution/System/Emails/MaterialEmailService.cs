using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.System.Emails;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.MaterialDistribution.Data;
using AutomationSystem.Main.Core.MaterialDistribution.Data.Models;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Emails.System.Models;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.MaterialDistribution.System.Emails
{
    public class MaterialEmailService : IMaterialEmailService
    {
        private readonly IEmailServiceHelper helper;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IClassMaterialDatabaseLayer materialDb;
        private readonly IIncidentLogger incidentLogger;
        private readonly IEmailTextResolverFactory emailTextResolverFactory;
        private readonly IEmailTemplateResolver emailTemplateResolver;
        private readonly IClassEmailParameterResolverFactory classEmailParameterResolverFactory;
        private readonly IMainEmailHelper mainHelper;
        private readonly IRegistrationEmailParameterResolverFactory registrationEmailParameterResolverFactory;
        private readonly IMaterialEmailParameterResolverFactory materialEmailParameterResolverFactory;

        // constructor
        public MaterialEmailService(
            IEmailServiceHelper helper,
            IRegistrationDatabaseLayer registrationDb,
            IPersonDatabaseLayer personDb,
            IClassMaterialDatabaseLayer materialDb,
            IIncidentLogger incidentLogger,
            IEmailTextResolverFactory emailTextResolverFactory,
            IEmailTemplateResolver emailTemplateResolver,
            IClassEmailParameterResolverFactory classEmailParameterResolverFactory,
            IMainEmailHelper mainHelper,
            IRegistrationEmailParameterResolverFactory registrationEmailParameterResolverFactory,
            IMaterialEmailParameterResolverFactory materialEmailParameterResolverFactory)
        {
            this.helper = helper;
            this.registrationDb = registrationDb;
            this.personDb = personDb;
            this.materialDb = materialDb;
            this.incidentLogger = incidentLogger;
            this.emailTextResolverFactory = emailTextResolverFactory;
            this.emailTemplateResolver = emailTemplateResolver;
            this.classEmailParameterResolverFactory = classEmailParameterResolverFactory;
            this.mainHelper = mainHelper;
            this.registrationEmailParameterResolverFactory = registrationEmailParameterResolverFactory;
            this.materialEmailParameterResolverFactory = materialEmailParameterResolverFactory;
        }

        public TracedSendResult<RecipientId> SendMaterialEmailsToRecipients(
              EmailTypeEnum emailTypeId,
              long classId,
              List<RecipientId> recipientIds,
              bool createIncident = false)
        {
            var result = new TracedSendResult<RecipientId>();
            result.IsSuccessful = false;
            RecipientId currentRecipientId = null;
            try
            {

                // loads materials recipient and class
                var material = materialDb.GetClassMaterialByClassId(classId, ClassMaterialIncludes.Class | ClassMaterialIncludes.ClassMaterialRecipients);
                if (material == null)
                {
                    throw new ArgumentException($"There is no Class material related to class with id {classId}.");
                }

                var cls = material.Class;
                var materialRecipients = material.ClassMaterialRecipients.ToDictionary(x => new RecipientId(x.RecipientTypeId, x.RecipientId));
                var languages = new HashSet<LanguageEnum>{cls.OriginLanguageId};
                if (cls.TransLanguageId.HasValue)
                {
                    languages.Add(cls.TransLanguageId.Value);
                }

                var templateMap = emailTemplateResolver.GetValidTemplates(
                    new EmailTemplateEntityId(EntityTypeEnum.MainProfile, cls.ProfileId), emailTypeId, languages).ToDictionary(x => x.LanguageId);


                // assembles text resolver
                var classResolver = classEmailParameterResolverFactory.CreateClassParameterResolver(cls);
                var materialResolver = materialEmailParameterResolverFactory.CreateClassMaterialRecipientParameterResolver();
                var registrationResolver = registrationEmailParameterResolverFactory.CreateRegistrationParameterResolver();
                var personResolver = registrationEmailParameterResolverFactory.CreatePersonAsRegistrationParameterResolver();
                var sender = mainHelper.GetSenderInfoByPersonId(cls.CoordinatorId);

                // process registrations
                var textResolver = emailTextResolverFactory.CreateEmailTextResolver(classResolver, registrationResolver, materialResolver);

                var registrationIds = recipientIds.Where(x => x.TypeId == EntityTypeEnum.MainClassRegistration).Select(x => x.Id).ToList();
                var registrations = registrationDb.GetRegistrationsByIds(registrationIds, ClassRegistrationIncludes.Addresses);
                foreach (var registration in registrations)
                {
                    currentRecipientId = new RecipientId(EntityTypeEnum.MainClassRegistration, registration.ClassRegistrationId);

                    // binds registration and materials
                    var materialRecipient = materialRecipients[currentRecipientId];
                    materialResolver.Bind(materialRecipient);
                    registrationResolver.Bind(registration);

                    // sends email
                    var studentEmail = mainHelper.GetRegistrationRecipientEmail(registration, RecipientType.Student);
                    var processedEmailPair = SendMaterialEmailToRecipient(currentRecipientId, registration.LanguageId, studentEmail, emailTypeId, sender, textResolver, templateMap);
                    result.ProcessedEmailIdEntityIdPairs.Add(processedEmailPair);

                    currentRecipientId = null;
                }

                // process persons
                textResolver = emailTextResolverFactory.CreateEmailTextResolver(classResolver, personResolver, materialResolver);

                var personIds = recipientIds.Where(x => x.TypeId == EntityTypeEnum.MainPerson).Select(x => x.Id).ToList();
                var persons = personDb.GetPersonsByIds(personIds, PersonIncludes.Address);
                foreach (var person in persons)
                {
                    currentRecipientId = new RecipientId(EntityTypeEnum.MainPerson, person.PersonId);

                    // binds person and materials
                    var materialRecipient = materialRecipients[currentRecipientId];
                    materialResolver.Bind(materialRecipient);
                    personResolver.Bind(person);

                    // sends email
                    var processedEmailPair = SendMaterialEmailToRecipient(currentRecipientId, LocalisationInfo.DefaultLanguage, person.Email, emailTypeId, sender, textResolver, templateMap);
                    result.ProcessedEmailIdEntityIdPairs.Add(processedEmailPair);

                    currentRecipientId = null;
                }

                result.IsSuccessful = true;                 // whole batch was processed, sending was successful
            }
            catch (Exception e)
            {
                result.Exception = e;
                if (!createIncident)
                {
                    return result;
                }

                var incident = IncidentForLog.New(IncidentTypeEnum.EmailError, e);
                if (currentRecipientId != null)
                {
                    incident.Entity(currentRecipientId);
                }
                else
                {
                    incident.Entity(EntityTypeEnum.MainClass, classId);
                }

                incidentLogger.LogIncident(incident);
            }
            return result;
        }

        #region private methods
        
        private Tuple<long, RecipientId> SendMaterialEmailToRecipient(
            RecipientId recipientId,
            LanguageEnum languageId,
            string recipientEmail,
            EmailTypeEnum emailTypeId,
            SenderInfo sender,
            IEmailTextResolver textResolver,
            Dictionary<LanguageEnum, EmailTemplate> templateMap)
        {
            // loads template
            if (!templateMap.TryGetValue(languageId, out var template))
            {
                throw new InvalidOperationException($"There is no email template {emailTypeId} for language {languageId}");
            }

            // sends email
            var emailId = helper.SendEmailForTemplate(
                template,
                textResolver,
                new EmailEntityId(recipientId.TypeId, recipientId.Id),
                recipientEmail,
                sender,
                (int)SeverityEnum.High);
            return new Tuple<long, RecipientId>(emailId, recipientId);
        }

        #endregion
    }
}
