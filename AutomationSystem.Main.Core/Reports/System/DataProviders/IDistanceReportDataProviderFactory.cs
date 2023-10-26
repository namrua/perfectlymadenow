using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;

namespace AutomationSystem.Main.Core.Reports.System.DataProviders
{
    public interface IDistanceReportDataProviderFactory
    {
        IDistanceReportDataProvider CreateDistanceReportDataProvider(DistanceCrfReportParameters parameters);
    }
}
