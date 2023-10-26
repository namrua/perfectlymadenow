using System;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;

namespace AutomationSystem.Main.Core.Reports.AppLogic.Convertors
{
    /// <summary>
    /// Converts general report service related objects
    /// </summary>
    public class GeneralReportServiceConvertor : IGeneralReportServiceConvertor
    {
        private readonly IPersonDatabaseLayer personDb;

        public GeneralReportServiceConvertor(IPersonDatabaseLayer personDb)
        {
            this.personDb = personDb;
        }

        public WwaCrfReportForEdit InitializeWwaCrfReportForEdit(ProfileFilter filter)
        {
            var distanceCoordinators = personDb.GetUsedDistanceCoordinators(filter.ProfileIds);
            var result = new WwaCrfReportForEdit
            {
                DistanceCoordinators = distanceCoordinators
            };
            return result;
        }

        public DistanceCrfReportParameters ConvertToDistanceCrfReportParameters(WwaCrfReportForm form, ProfileFilter profileFilter)
        {
            var defaultDate = default(DateTime);
            var result = new DistanceCrfReportParameters
            {
                FromDate = form.FromDate ?? defaultDate,
                ToDate = form.ToDate ?? defaultDate,
                DistanceCoordinatorId = form.DistanceCoordinatorId ?? 0,
                ProfileIds = profileFilter.ProfileIds
            };
            return result;
        }
    }
}
