using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Emails.System;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Handlers;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Main.Model;
using System;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.Registrations.System
{
    public class RegistrationCommandService : IRegistrationCommandService
    {
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IRegistrationEmailService registrationEmailService;
        private readonly IEmailTypeResolver emailTypeResolver;
        private readonly IClassMaterialDistributionHandler materialDistributionHandler;
        private readonly IMainAsyncRequestManager requestManager;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly IClassTypeResolver classTypeResolver;

        public RegistrationCommandService(
            IRegistrationDatabaseLayer registrationDb,
            IRegistrationEmailService registrationEmailService,
            IEmailTypeResolver emailTypeResolver,
            IClassMaterialDistributionHandler materialDistributionHandler,
            IMainAsyncRequestManager requestManager,
            IRegistrationTypeResolver registrationTypeResolver,
            IClassTypeResolver classTypeResolver)
        {
            this.registrationDb = registrationDb;
            this.registrationEmailService = registrationEmailService;
            this.emailTypeResolver = emailTypeResolver;
            this.materialDistributionHandler = materialDistributionHandler;
            this.requestManager = requestManager;
            this.registrationTypeResolver = registrationTypeResolver;
            this.classTypeResolver = classTypeResolver;
        }

        public void ApproveRegistration(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            ApproveRegistration(registration);
        }

        public void ApproveRegistration(ClassRegistration registration)
        {
            CompleteManualReviewing(registration);
            registration = registrationDb.ApproveClassRegistration(registration.ClassRegistrationId);
            requestManager.AddIntegrationRequestForClassRegistration(registration, AsyncRequestTypeEnum.AddToOuterSystem, (int)SeverityEnum.High);

            if (classTypeResolver.AreAutomationNotificationsAllowed(registration.Class.ClassCategoryId))
            {
                registrationEmailService.SendRegistrationEmailByType(
                    emailTypeResolver.ResolveEmailTypeForRegistration(EmailTypeEnum.RegistrationConfirmation, registration.RegistrationTypeId),
                    registration.ClassRegistrationId, RecipientType.Student);
            }

            if (registration.Class.ClassCategoryId == ClassCategoryEnum.DistanceClass)
            {
                registrationEmailService.SendRegistrationEmailByType(EmailTypeEnum.WwaStudentRegistrationNotification,
                    registration.ClassRegistrationId, RecipientType.Coordinator, true);
            }

            materialDistributionHandler.HandleRegistrationAprovement(registration, false);
        }

        #region private methods

        private ClassRegistration GetRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class registration with id {registrationId}.");
            }

            return result;
        }

        private void CompleteManualReviewing(ClassRegistration registration)
        {
            if (registration.Class == null)
            {
                throw new InvalidOperationException("Class is not included into ClassRegistration object.");
            }

            if (registrationTypeResolver.NeedsReview(registration.RegistrationTypeId, registration.Class.ClassTypeId)
                && !registration.IsReviewed)
            {
                registrationDb.SetRegistrationToReviewed(registration.ClassRegistrationId);
            }
        }

        #endregion
    }
}
