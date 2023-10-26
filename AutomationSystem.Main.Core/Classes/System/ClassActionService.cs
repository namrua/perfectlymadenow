using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.FormerClasses.AppLogic;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.EntityIntegration.System;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Classes.System.Emails;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.Classes.System
{
    public class ClassActionService : IClassActionService
    {
        private readonly IClassActionConvertor classActionConvertor;
        private readonly IEmailIntegration emailIntegration;
        private readonly IClassDatabaseLayer classDb;
        private readonly IGenericEntityIntegrationProvider integrationProvider;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IFormerClassPropagator formerClassPropagator;
        private readonly IMainAsyncRequestManager asyncRequestManager;
        private readonly IEmailAttachmentProviderFactory emailAttachmentProviderFactory;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly IClassEmailService classEmailService;
        private readonly IClassTypeResolver classTypeResolver;

        public ClassActionService(
            IClassActionConvertor classActionConvertor,
            IEmailIntegration emailIntegration,
            IClassDatabaseLayer classDb,
            IGenericEntityIntegrationProvider integrationProvider,
            IPersonDatabaseLayer personDb,
            IRegistrationDatabaseLayer registrationDb,
            IFormerClassPropagator formerClassPropagator,
            IMainAsyncRequestManager asyncRequestManager,
            IEmailAttachmentProviderFactory emailAttachmentProviderFactory,
            IRegistrationTypeResolver registrationTypeResolver,
            IClassEmailService classEmailService,
            IClassTypeResolver classTypeResolver)
        {
            this.classActionConvertor = classActionConvertor;
            this.emailIntegration = emailIntegration;
            this.classDb = classDb;
            this.integrationProvider = integrationProvider;
            this.personDb = personDb;
            this.registrationDb = registrationDb;
            this.formerClassPropagator = formerClassPropagator;
            this.asyncRequestManager = asyncRequestManager;
            this.emailAttachmentProviderFactory = emailAttachmentProviderFactory;
            this.registrationTypeResolver = registrationTypeResolver;
            this.classEmailService = classEmailService;
            this.classTypeResolver = classTypeResolver;
        }

        public long CreateClassAction(Class cls, ClassActionTypeEnum classActionTypeId)
        {
            var clonedTemplates = new List<EmailTemplate>();
            if (CanCreateEmailTemplatesForAction(cls.ClassCategoryId, classActionTypeId))
            {
                // gets cloned email templates
                var emailTypeIds = classActionConvertor.GetEmailTypeIdsByClassActionTypeId(classActionTypeId, cls);
                clonedTemplates = emailTypeIds.SelectMany(x => emailIntegration.CloneEmailTemplates(
                    x,
                    new EmailTemplateEntityId(EntityTypeEnum.MainProfile, cls.ProfileId),
                    cls.OriginLanguageId,
                    cls.TransLanguageId)).ToList();
            }

            // class action to insert and inserts it to database
            var toInsert = new ClassAction
            {
                ClassId = cls.ClassId,
                ClassActionTypeId = classActionTypeId,
            };
            var result = classDb.InsertClassAction(toInsert);

            // saves cloned email templates and returns classActionId
            emailIntegration.SaveClonedEmailTemplates(clonedTemplates, new EmailTemplateEntityId(EntityTypeEnum.MainClassAction, result));
            return result;
        }

        public void ProcessClassAction(long classActionId)
        {
            var classAction = classDb.GetClassActionById(classActionId, ClassActionIncludes.ClassClassStyle);
            if (classAction == null)
            {
                throw new ArgumentException($"There is no class action with id {classActionId}.");
            }

            ProcessClassAction(classAction);
        }

        public void ProcessClassAction(ClassAction classAction)
        {
            var classActionId = classAction.ClassActionId;
            var registrationRestrictions = classActionConvertor.GetRegistrationRestrictionByClassActionTypeId(classAction.ClassActionTypeId);

            switch (classAction.ClassActionTypeId)
            {
                case ClassActionTypeEnum.Change:
                case ClassActionTypeEnum.WwaChange:
                    classEmailService.SendClassActionEmails(classActionId, registrationRestrictions, null, null, true);
                    break;

                case ClassActionTypeEnum.Cancelation:
                    ProcessCancellationClassAction(classAction, registrationRestrictions);
                    break;

                case ClassActionTypeEnum.Completion:
                    ProcessCompletionClassAction(classAction, registrationRestrictions);
                    break;

            }
            classDb.SetClassActionAsProcessed(classActionId);
        }

        #region private methods

        private void ProcessCancellationClassAction(ClassAction classAction, ClassActionRegistrationRestriction registrationRestrictions)
        {
            classDb.SetClassAsCanceled(classAction.ClassId);

            if (classTypeResolver.AreAutomationNotificationsAllowed(classAction.Class.ClassCategoryId))
            {
                classEmailService.SendClassActionEmails(classAction.ClassActionId, registrationRestrictions, null, null, true);
            }

            integrationProvider.DetachEntity(EntityTypeEnum.MainClass, classAction.ClassId);
        }

        private void ProcessCompletionClassAction(ClassAction classAction, ClassActionRegistrationRestriction registrationRestriction)
        {
            // loads all registrations
            var classId = classAction.ClassId;
            var registrationFilter = new RegistrationFilter
            {
                ClassId = classId,
                RegistrationState = RegistrationState.Approved,
            };
            var registrations = registrationDb.GetRegistrationsByFilter(registrationFilter,
                ClassRegistrationIncludes.ClassRegistrationFiles | ClassRegistrationIncludes.Addresses);

            // loads persons - persons are ignored for distant classes
            var persons = new List<Person>();
            if (classTypeResolver.AreCertificatesAllowedForClassPersons(classAction.Class.ClassCategoryId))
            {
                var personIds = new HashSet<long>();
                if (classAction.Class.GuestInstructorId.HasValue)
                    personIds.Add(classAction.Class.GuestInstructorId.Value);
                var classPersons = classDb.GetClassPersonsByClassIdAndRoles(classId, PersonRoleTypeEnum.ApprovedStaff, PersonRoleTypeEnum.Instructor);
                personIds.UnionWith(classPersons.Select(x => x.PersonId));
                persons = personDb.GetPersonsByIds(personIds, PersonIncludes.Address);
            }

            if (classTypeResolver.AreAutomationNotificationsAllowed(classAction.Class.ClassCategoryId))
            {
                var certificateAttachments = GetCertificatesForAttachment(classAction.Class, registrations, persons);
                classEmailService.SendClassActionEmails(classAction.ClassActionId, registrationRestriction, certificateAttachments, persons, true);
            }

            if (classTypeResolver.IsPropagationToFormerClassesAllowed(classAction.Class.ClassCategoryId))
            {
                formerClassPropagator.PropagateToFormerDatabase(classAction.Class, registrations);
            }

            if (classTypeResolver.AreReportsAllowed(classAction.Class.ClassCategoryId))
            {
                asyncRequestManager.AddDocumentRequestForClass(classId, AsyncRequestTypeEnum.SendFinalReports, (int)SeverityEnum.High);
            }

            // set class as complete          
            classDb.SetClassAsFinished(classId);
        }

        private IEmailAttachmentProvider GetCertificatesForAttachment(Class cls, List<ClassRegistration> registrations, List<Person> persons)
        {
            // checks whether certificates are allowed
            var result = emailAttachmentProviderFactory.CreateMapEmailAttachmentProvider();
            if (!classTypeResolver.AreCertificatesAllowed(cls.ClassCategoryId))
            {
                return result;
            }

            // when  SendCertificatesByEmail is set to false, only certificates for WWA students are attached
            if (!cls.ClassStyle.SendCertificatesByEmail)
            {
                persons = new List<Person>();
                registrations = registrations.Where(x => registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId)).ToList();
            }

            // checks certificates for registration and adds them as attachment
            foreach (var registration in registrations)
            {
                var certificate = registration.ClassRegistrationFiles.FirstOrDefault(x => x.Code == MainFileReservedNames.Certificate);
                if (certificate == null)
                    throw new InvalidOperationException($"The certificate for Class registration with id {registration.ClassRegistrationId} is not generated.");

                result.AddFileId(EntityTypeEnum.MainClassRegistration, registration.ClassRegistrationId, certificate.FileId);
            }

            // checks certificates for persons and adds them as attachment
            var classFiles = classDb.GetClassFilesByClassId(cls.ClassId);
            foreach (var person in persons)
            {
                var certificate = classFiles.FirstOrDefault(x => x.Code == $"{MainFileReservedNames.Certificate}-{person.PersonId}");
                if (certificate == null)
                    throw new InvalidOperationException($"The certificate for Person with id {person.PersonId} is not generated.");

                result.AddFileId(EntityTypeEnum.MainPerson, person.PersonId, certificate.FileId);
            }
            return result;
        }

        private bool CanCreateEmailTemplatesForAction(ClassCategoryEnum classCategoryId, ClassActionTypeEnum classActionTypeId)
        {
            return classTypeResolver.AreAutomationNotificationsAllowed(classCategoryId)
                   || classActionTypeId != ClassActionTypeEnum.Cancelation && classActionTypeId != ClassActionTypeEnum.Completion;
        }

        #endregion
    }
}
