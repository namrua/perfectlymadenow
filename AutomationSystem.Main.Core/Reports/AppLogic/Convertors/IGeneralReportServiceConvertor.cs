using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;

namespace AutomationSystem.Main.Core.Reports.AppLogic.Convertors
{
    /// <summary>
    /// Converts general report service related objects
    /// </summary>
    public interface IGeneralReportServiceConvertor
    {
        WwaCrfReportForEdit InitializeWwaCrfReportForEdit(ProfileFilter profileFilter);

        DistanceCrfReportParameters ConvertToDistanceCrfReportParameters(WwaCrfReportForm form, ProfileFilter profileFilter);
    }
}
