using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Reports.AppLogic;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Main.Core.Reports.AppLogic.Convertors;
using AutomationSystem.Main.Core.Reports.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Files.System.Models;
using System;
using System.Linq;

namespace AutomationSystem.Main.Core.Reports.AppLogic
{
    /// <summary>
    /// Provides general reporting
    /// </summary>
    public class GeneralReportService : IGeneralReportService
    {
        private readonly IReportService reportService;
        private readonly IDistanceReportService distanceReportService;
        private readonly IClassDatabaseLayer classDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IGeneralReportServiceConvertor grsConvertor;

        public GeneralReportService(
            IReportService reportService,
            IDistanceReportService distanceReportService,
            IClassDatabaseLayer classDb,
            IIdentityResolver identityResolver,
            IGeneralReportServiceConvertor grsConvertor)
        {
            this.reportService = reportService;
            this.distanceReportService = distanceReportService;
            this.classDb = classDb;
            this.identityResolver = identityResolver;
            this.grsConvertor = grsConvertor;
        }

        public WwaCrfReportForEdit GetNewWwaCrfReportForEdit()
        {
            var profileFilter = identityResolver.GetGrantedProfilesForEntitle(Entitle.MainDistanceClasses);
            var result = grsConvertor.InitializeWwaCrfReportForEdit(profileFilter);
            if (result.DistanceCoordinators.Count == 1)
                result.Form.DistanceCoordinatorId = result.DistanceCoordinators.First().PersonId;
            return result;
        }

        public WwaCrfReportForEdit GetWwaCrfReportForEditFromForm(WwaCrfReportForm form)
        {
            var profileFilter = identityResolver.GetGrantedProfilesForEntitle(Entitle.MainDistanceClasses);
            var result = grsConvertor.InitializeWwaCrfReportForEdit(profileFilter);
            result.Form = form;
            return result;
        }

        public FileForDownload GenerateWwaCrfReport(string rootPath, WwaCrfReportForm form)
        {
            // process request
            var profileFilter = identityResolver.GetGrantedProfilesForEntitle(Entitle.MainDistanceClasses);
            var crfParams = grsConvertor.ConvertToDistanceCrfReportParameters(form, profileFilter);
            var result = distanceReportService.GenerateWwaCrfReport(rootPath, crfParams);
            return result;
        }

        public FileForDownload GetClassReportByType(ClassReportType reportType, string rootPath, long classId)
        {
            var toCheck = GetClassById(classId);
            identityResolver.CheckEntitleForClass(toCheck);

            var result = reportService.GetClassReportByType(reportType, rootPath, classId);
            return result;
        }

        #region private methods

        public Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
                throw new ArgumentException($"There is no Class with id {classId}.");
            return result;
        }

        #endregion
    }
}
