using AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Main.Core.Reports.System
{
    /// <summary>
    /// Provides report services for distance classes
    /// Not intended for usage by controllers
    /// </summary>
    public interface IDistanceReportService
    {
        FileForDownload GenerateWwaCrfReport(string rootPath, DistanceCrfReportParameters parameters);
    }
}