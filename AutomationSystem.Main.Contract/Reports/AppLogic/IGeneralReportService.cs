using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Shared.Contract.Files.System.Models;

namespace AutomationSystem.Main.Contract.Reports.AppLogic
{
    /// <summary>
    /// Provides global reports from administration pages
    /// Can be called directly from controllers
    /// </summary>
    public interface IGeneralReportService
    {
        WwaCrfReportForEdit GetNewWwaCrfReportForEdit();

        WwaCrfReportForEdit GetWwaCrfReportForEditFromForm(WwaCrfReportForm form);

        FileForDownload GenerateWwaCrfReport(string rootPath, WwaCrfReportForm form);

        FileForDownload GetClassReportByType(ClassReportType reportType, string rootPath, long classId);
    }
}