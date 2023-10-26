using AutomationSystem.Main.Core.Reports.System.DataProviders;
using AutomationSystem.Main.Core.Reports.System.ReportAvailabilityResolvers;

namespace AutomationSystem.Main.Core.Reports.System.Models.ReportService
{
    /// <summary>
    /// Collects component used for generating report for specified class
    /// </summary>
    public interface IClassReportComponents
    {
        IClassDataProvider Data { get; }
        ICommonReportDataProvider Common { get; }
        IFinancialFormsDataProvider Financial { get; }
        IReportAvailabilityResolver AvailabilityResolver { get; }
    }
}