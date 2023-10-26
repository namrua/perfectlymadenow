using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Guest Instructor Closing Statement report generator
    /// </summary>
    public class GuestInstructorClosingStatementReportGenerator : BaseExcelReportGenerator
    {
        private const string reportDefinition = "GuestInstructorStatement.xml";

        public GuestInstructorClosingStatementReportGenerator()
            : base(ClassReportType.GuestInstructorClosingStatement, MainFileReservedNames.GuestInstructorStatement, ReportConstants.GuestInstructorClosingStatement)
        {
        }

        public override byte[] GenerateReport(IClassReportComponents components, string rootPath, string reportFolder)
        {
            // creating 
            var model = components.Financial.GetGuestInstructorStatementModel();
            var writer = GetSheetCreator(rootPath, reportFolder, reportDefinition);
            var content = writer.CreateSheet(model);
            return content;
        }
    }
}
