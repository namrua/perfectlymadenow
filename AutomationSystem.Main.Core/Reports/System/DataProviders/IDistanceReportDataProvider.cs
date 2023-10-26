using AutomationSystem.Main.Core.Reports.System.Models.DistanceCrf;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    /// <summary>
    /// Provides models for distance reports
    /// </summary>
    public interface IDistanceReportDataProvider
    {
        DistanceCrfReport GetDistanceCrfReportModel();
    }
}
