using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// CRF Lecture report generator
    /// </summary>
    public class CrfLectureReportGenerator : BaseExcelReportGenerator
    {
        private const string reportDefinition = "CRFLectureReport.xml";

        public CrfLectureReportGenerator()
            : base(ClassReportType.CrfLecture, MainFileReservedNames.FinancialCrfLecture, ReportConstants.CrfLecture)
        {
        }

        public override byte[] GenerateReport(IClassReportComponents components, string rootPath, string reportFolder)
        {
            // creating 
            var model = components.Common.GetCrfReportModel();
            var writer = GetSheetCreator(rootPath, reportFolder, reportDefinition);
            var content = writer.CreateSheet(model);
            return content;
        }
    }
}
