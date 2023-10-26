using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Preferences.System;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.Data.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.Classes.System.Emails
{
    public class ClassEmailService : IClassEmailService
    {

        public static readonly string[] MainLocalisationParams = { "RegistrationListNote" };

        private readonly IEmailServiceHelper helper;
        private readonly IEmailDatabaseLayer emailDb;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IMainPreferenceProvider mainPreferences;
        private readonly ICoreEmailParameterResolverFactory coreParameterResolverFactory;
        private readonly IEmailTextResolverFactory emailTextResolverFactory;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly IEmailTypeResolver emailTypeResolver;
        private readonly IEmailTemplateResolver emailTemplateResolver;
        private readonly IClassEmailParameterResolverFactory classEmailParameterResolverFactory;
        private readonly IMainEmailHelper mainHelper;
        private readonly IRegistrationEmailParameterResolverFactory registrationEmailParameterResolverFactory;

        // constructor
        public ClassEmailService(
            IEmailServiceHelper helper,
            IEmailDatabaseLayer emailDb,
            IRegistrationDatabaseLayer registrationDb,
            IClassDatabaseLayer classDb,
            IPersonDatabaseLayer personDb,
            IMainPreferenceProvider mainPreferences,
            ICoreEmailParameterResolverFactory coreParameterResolverFactory,
            IEmailTextResolverFactory emailTextResolverFactory,
            IRegistrationTypeResolver registrationTypeResolver,
            IEmailTypeResolver emailTypeResolver,
            IEmailTemplateResolver emailTemplateResolver,
            IClassEmailParameterResolverFactory classEmailParameterResolverFactory,
            IMainEmailHelper mainHelper,
            IRegistrationEmailParameterResolverFactory registrationEmailParameterResolverFactory)
        {
            this.helper = helper;
            this.emailDb = emailDb;
            this.registrationDb = registrationDb;
            this.classDb = classDb;
            this.personDb = personDb;
            this.mainPreferences = mainPreferences;
            this.coreParameterResolverFactory = coreParameterResolverFactory;
            this.emailTextResolverFactory = emailTextResolverFactory;
            this.registrationTypeResolver = registrationTypeResolver;
            this.emailTypeResolver = emailTypeResolver;
            this.emailTemplateResolver = emailTemplateResolver;
            this.classEmailParameterResolverFactory = classEmailParameterResolverFactory;
            this.mainHelper = mainHelper;
            this.registrationEmailParameterResolverFactory = registrationEmailParameterResolverFactory;
        }
        
        public List<long> SendClassActionEmails(
            long classActionId,
            ClassActionRegistrationRestriction restriction,
            IEmailAttachmentProvider attachments = null,
            List<Person> persons = null,
            bool sealTemplate = false)
        {
            // loads email template map
            var templateMap = GetEmailTemplateLanguageWwaMap(EntityTypeEnum.MainClassAction, classActionId);

            // loads class action with class
            var classAction = classDb.GetClassActionById(classActionId, ClassActionIncludes.Class);
            if (classAction == null)
            {
                throw new ArgumentException($"There is no class action with id {classActionId}.");
            }

            // loads registration of conversation
            var registrationFilter = new RegistrationFilter
            {
                ClassId = classAction.ClassId,
                RegistrationState = RegistrationState.Approved,
            };
            var registrations = registrationDb.GetRegistrationsByFilter(registrationFilter, ClassRegistrationIncludes.Addresses);
            registrations = ApplyRestrictionsToRegistrations(registrations, restriction);

            // assembles text resolver
            var classResolver = classEmailParameterResolverFactory.CreateClassParameterResolver(classAction.Class);
            var registrationResolver = registrationEmailParameterResolverFactory.CreateRegistrationParameterResolver();
            var textResolver = emailTextResolverFactory.CreateEmailTextResolver(classResolver, registrationResolver);

            // gets sender
            var sender = mainHelper.GetSenderInfoByPersonId(classAction.Class.CoordinatorId);

            // process emails
            var result = new List<long>();
            foreach (var registration in registrations)
            {
                // loads template
                var key = new LanguageWwaKey(registration.LanguageId, registrationTypeResolver.IsWwaRegistration(registration.RegistrationTypeId));
                if (!templateMap.TryGetValue(key, out var template))
                {
                    throw new InvalidOperationException($"There is no email template for class action {classActionId} for language {registration.LanguageId} and isWWA = {key.IsWwa}.");
                }

                // binds registration
                registrationResolver.Bind(registration);

                // sends email
                var emailId = helper.SendEmailForTemplate(template, textResolver,
                    new EmailEntityId(EntityTypeEnum.MainClassRegistration, registration.ClassRegistrationId),
                    mainHelper.GetRegistrationRecipientEmail(registration, RecipientType.Student),
                    sender, (int)SeverityEnum.High, attachments);
                result.Add(emailId);
            }

            // process persons
            if (persons != null && persons.Any())
            {
                // assembles text resolver                
                var personResolver = registrationEmailParameterResolverFactory.CreatePersonAsRegistrationParameterResolver();
                var personTextResolver = emailTextResolverFactory.CreateEmailTextResolver(classResolver, personResolver);

                // loads template
                if (!templateMap.TryGetValue(new LanguageWwaKey(LocalisationInfo.DefaultLanguage, false),
                    out var template))
                {
                    throw new InvalidOperationException($"There is no email template for class action {classActionId} for language {LocalisationInfo.DefaultLanguage} and non-WWA.");
                }

                foreach (var person in persons)
                {
                    // binds registration
                    personResolver.Bind(person);

                    // sends email
                    var emailId = helper.SendEmailForTemplate(template, personTextResolver,
                        new EmailEntityId(EntityTypeEnum.MainPerson, person.PersonId),
                        person.Email, sender, (int)SeverityEnum.High, attachments);
                    result.Add(emailId);
                }
            }

            // seals template
            if (sealTemplate)
            {
                emailDb.SetEmailTemplatesToSealed(templateMap.Values.Select(x => x.EmailTemplateId));
            }

            return result;
        }

        public List<long> SendClassTextMapEmailByTypeToPersons(
            EmailTypeEnum emailTemplateId,
            long classId,
            Dictionary<string, object> textMap,
            IEnumerable<long> recipientPersons)
        {
            // loads class and persons
            var cls = classDb.GetClassById(classId);
            if (cls == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }

            var recipients = personDb.GetPersonsByIds(recipientPersons);

            // loads email template
            var template = emailTemplateResolver.GetEmailTemplateByTypeAndLanguage(emailTemplateId);

            // assembles resolvers
            var classResolver = classEmailParameterResolverFactory.CreateClassParameterResolver(cls);
            var localisationResolver = coreParameterResolverFactory.CreateLocalisedParameterResolver(MainLocalisationParams);
            var textMapResolver = coreParameterResolverFactory.CreateTextMapParameterResolver();
            textMapResolver.Bind(textMap);
            var textResolver = emailTextResolverFactory.CreateEmailTextResolver(classResolver, localisationResolver, textMapResolver);

            // gets sender
            var sender = mainHelper.GetSenderInfoByPersonId(cls.CoordinatorId);

            // sends emails to all recipientes
            var result = new List<long>();
            foreach (var recipient in recipients)
            {
                var emailId = helper.SendEmailForTemplate(template, textResolver,
                    new EmailEntityId(EntityTypeEnum.MainPerson, recipient.PersonId),
                    recipient.Email, sender, (int)SeverityEnum.High);
                result.Add(emailId);
            }

            return result;
        }

        public long SendClassTextMapEmailByTypeToRecipient(
            EmailTypeEnum emailTemplateId,
            long classId, Dictionary<string, object> textMap,
            RecipientType recipientType,
            IEmailAttachmentProvider attachments = null)
        {
            // loads class and persons
            var cls = classDb.GetClassById(classId);
            if (cls == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }

            // loads email template
            var template = emailTemplateResolver.GetEmailTemplateByTypeAndLanguage(emailTemplateId);

            // assembles resolvers
            var classResolver = classEmailParameterResolverFactory.CreateClassParameterResolver(cls);
            var localisationResolver = coreParameterResolverFactory.CreateLocalisedParameterResolver(MainLocalisationParams);
            var textMapResolver = coreParameterResolverFactory.CreateTextMapParameterResolver();
            textMapResolver.Bind(textMap);
            var textResolver = emailTextResolverFactory.CreateEmailTextResolver(classResolver, localisationResolver, textMapResolver);

            // sends emails to recipient
            var recipient = GetClassRecipientEmail(cls, recipientType);

            // gets sender
            var sender = mainHelper.GetSenderInfoByPersonId(cls.CoordinatorId);

            var result = helper.SendEmailForTemplate(template, textResolver,
                new EmailEntityId(EntityTypeEnum.MainClass, classId),
                recipient, sender, (int)SeverityEnum.High, attachments);
            return result;
        }
        
        #region private methods

        private Dictionary<LanguageWwaKey, EmailTemplate> GetEmailTemplateLanguageWwaMap(
            EntityTypeEnum entityTypeId, long entityId, EmailTypeEnum? emailTypeId = null)
        {
            var filter = new EmailTemplateFilter
            {
                IsDefault = false,
                IsValidated = true,
                EmailTemplateEntityId = new EmailTemplateEntityId(entityTypeId, entityId),
                EmailTypeId = emailTypeId
            };
            // if this causes exception, db is in inconsistent state
            var result = emailDb.GetEmailTemplatesByFilter(filter).ToDictionary(
                x => new LanguageWwaKey(x.LanguageId, emailTypeResolver.IsWwaEmailType(x.EmailTypeId)),
                y => y, new LanguageWwaKeyEqualityComparer());
            return result;
        }

        private List<ClassRegistration> ApplyRestrictionsToRegistrations(List<ClassRegistration> registrations, ClassActionRegistrationRestriction restriction)
        {
            switch (restriction)
            {
                case ClassActionRegistrationRestriction.None:
                    return registrations;

                case ClassActionRegistrationRestriction.OnlyCoordinated:
                    return registrations.Where(x => !registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId)).ToList();

                case ClassActionRegistrationRestriction.OnlyWwa:
                    return registrations.Where(x => registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId)).ToList();

                default:
                    throw new ArgumentOutOfRangeException(nameof(restriction), restriction, "Unknown ClassActionRegistrationRestriction");
            }
        }

        
        private string GetClassRecipientEmail(Class cls, RecipientType type)
        {
            string result = null;
            switch (type)
            {

                // class coordinator
                case RecipientType.Coordinator:
                    var coordinator = personDb.GetPersonById(cls.CoordinatorId);
                    if (coordinator != null)
                        result = coordinator.Email;
                    break;

                case RecipientType.MasterCoordinator:
                    result = mainPreferences.GetMasterCoordinatorEmail();
                    break;

                case RecipientType.Administrator:
                    result = helper.GetAdminRecipient();
                    break;

                case RecipientType.Helpdesk:
                    result = helper.GetHelpdeskEmail();
                    break;
            }

            if (result == null)
            {
                throw new InvalidOperationException(
                    $"Cannot obtain recipient's email by type {type} from Class with id {cls.ClassId}.");
            }

            return result;
        }

        #endregion
    }
}
