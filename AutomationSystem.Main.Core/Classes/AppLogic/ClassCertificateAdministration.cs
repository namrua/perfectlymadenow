using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Certificates;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Model;
using System;
using System.Linq;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public class ClassCertificateAdministration : IClassCertificateAdministration
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IMainFileService mainFileService;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IMainAsyncRequestManager asyncRequestManager;
        private readonly IIdentityResolver identityResolver;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly IClassTypeResolver classTypeResolver;

        public ClassCertificateAdministration(
            IClassDatabaseLayer classDb,
            IMainFileService mainFileService,
            IRegistrationDatabaseLayer registrationDb,
            IMainAsyncRequestManager asyncRequestManager,
            IIdentityResolver identityResolver,
            IRegistrationTypeResolver registrationTypeResolver,
            IClassTypeResolver classTypeResolver)
        {
            this.classDb = classDb;
            this.mainFileService = mainFileService;
            this.registrationDb = registrationDb;
            this.asyncRequestManager = asyncRequestManager;
            this.identityResolver = identityResolver;
            this.registrationTypeResolver = registrationTypeResolver;
            this.classTypeResolver = classTypeResolver;
        }

        public ClassCertificatesPageModel GetClassCertificatesPageModel(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassType);
            identityResolver.CheckEntitleForClass(cls);

            var result = new ClassCertificatesPageModel();
            result.Class = ClassConvertor.ConvertToClassShortDetial(cls);

            // checks whether reports are allowed
            if (!classTypeResolver.AreCertificatesAllowed(cls.ClassCategoryId))
            {
                result.AreCertificatesDisabled = true;
                result.CertificatesDisabledMessage = "Certificates are not available for the class.";
                return result;
            }

            // class fiels and request
            result.StaffsAndComposed = mainFileService.GetClassFileDetails(classId)
                .Where(x => MainFileReservedNames.IsCertificateCode(x.Code)).ToList();
            result.GeneratingRequests = asyncRequestManager.GetLastRequestsByEntityAndTypes(EntityTypeEnum.MainClass,
                classId, AsyncRequestTypeEnum.GenerateCertificates);

            // fills registrations
            var filter = new RegistrationFilter
            {
                ClassId = classId,
                RegistrationState = RegistrationState.Approved
            };
            var registrations = registrationDb.GetRegistrationsByFilter(filter, ClassRegistrationIncludes.ClassRegistrationFiles);
            result.Students = registrations.Where(x => !registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId))
                .SelectMany(x => x.ClassRegistrationFiles).Where(x => MainFileReservedNames.IsCertificateCode(x.Code))
                .Select(mainFileService.ConvertToEntityFileDetail).ToList();
            result.WwaStudents = registrations.Where(x => registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId))
                .SelectMany(x => x.ClassRegistrationFiles).Where(x => MainFileReservedNames.IsCertificateCode(x.Code))
                .Select(mainFileService.ConvertToEntityFileDetail).ToList();
            return result;
        }

        public long GenerateCertificates(long classId)
        {
            var toCheck = GetClassById(classId);
            identityResolver.CheckEntitleForClass(toCheck);

            var result = asyncRequestManager.AddDocumentRequestForClass(classId, AsyncRequestTypeEnum.GenerateCertificates, (int)SeverityEnum.High);
            return result.AsyncRequestId;
        }

        #region private methods

        public Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }

            return result;
        }

        #endregion
    }
}