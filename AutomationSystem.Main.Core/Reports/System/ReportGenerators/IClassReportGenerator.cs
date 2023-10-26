using AutomationSystem.Main.Core.Reports.System.Models.ReportService;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Class report generator
    /// </summary>
    public interface IClassReportGenerator
    {
        ClassReportTypeInfo TypeInfo { get; }

        byte[] GenerateReport(IClassReportComponents components, string rootPath, string reportFolder);
    }
}