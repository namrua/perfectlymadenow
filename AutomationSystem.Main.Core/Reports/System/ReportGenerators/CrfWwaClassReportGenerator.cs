using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// CRF WWA class report
    /// </summary>
    public class CrfWwaClassReportGenerator : BaseExcelReportGenerator
    {
        private const string reportDefinition = "WWAReport.xml";

        public CrfWwaClassReportGenerator()
            : base(ClassReportType.CrfWwaClass, MainFileReservedNames.FinancialWwa, ReportConstants.CrfWwaClass)
        {
        }

        public override byte[] GenerateReport(IClassReportComponents components, string rootPath, string reportFolder)
        {
            // creating 
            var model = components.Common.GetWwaReportModel();
            var writer = GetSheetCreator(rootPath, reportFolder, reportDefinition);
            var content = writer.CreateSheet(model);
            return content;
        }
    }
}
