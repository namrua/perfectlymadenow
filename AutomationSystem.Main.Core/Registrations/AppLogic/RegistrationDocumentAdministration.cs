using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Documents;
using AutomationSystem.Main.Core.Certificates.System;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Model;
using System;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    public class RegistrationDocumentAdministration : IRegistrationDocumentAdministration
    {
        private readonly ICertificateService certificateService;
        private readonly IIdentityResolver identityResolver;
        private readonly IMainFileService mainFileService;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IClassTypeResolver classTypeResolver;

        public RegistrationDocumentAdministration(
            ICertificateService certificateService,
            IIdentityResolver identityResolver,
            IMainFileService mainFileService,
            IRegistrationDatabaseLayer registrationDb,
            IClassTypeResolver classTypeResolver)
        {
            this.certificateService = certificateService;
            this.identityResolver = identityResolver;
            this.mainFileService = mainFileService;
            this.registrationDb = registrationDb;
            this.classTypeResolver = classTypeResolver;
        }

        public void GenerateCertificate(string rootPath, long registrationId)
        {
            var toCheck = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(toCheck.Class);
            
            certificateService.GenerateCertificate(rootPath, registrationId);
        }

        
        public RegistrationDocumentsPageModel GetRegistrationDocumentsPageModel(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);

            var result = new RegistrationDocumentsPageModel
            {
                ClassId = registration.ClassId,
                ClassRegistrationId = registration.ClassRegistrationId,
                AreCertificatesAllowed = classTypeResolver.AreCertificatesAllowed(registration.Class.ClassCategoryId),
                Files = mainFileService.GetClassRegistrationFileDetails(registrationId)
            };
            return result;
        }

        #region private methods
        private ClassRegistration GetRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
                throw new ArgumentException($"There is no Class registration with id {registrationId}.");
            return result;
        }
        #endregion
    }
}
