using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Fa Closing Statement report generator
    /// </summary>
    public class FaClosingStatementReportGenerator : BaseExcelReportGenerator
    {
        private const string reportDefinition = "FaClosingStatement.xml";

        public FaClosingStatementReportGenerator()
            : base(ClassReportType.FaClosingStatement, MainFileReservedNames.FaClosingStatement, ReportConstants.FaClosingStatement)
        {
        }

        public override byte[] GenerateReport(IClassReportComponents components, string rootPath, string reportFolder)
        {
            // creating 
            var model = components.Financial.GetFaClosingStatementModel();
            var writer = GetSheetCreator(rootPath, reportFolder, reportDefinition);
            var content = writer.CreateSheet(model);
            return content;
        }
    }
}
