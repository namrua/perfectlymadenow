using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Crf Class report generator
    /// </summary>
    public class CrfClassReportGenerator : BaseExcelReportGenerator
    {
        private const string reportDefinition = "CRFReport.xml";

        public CrfClassReportGenerator() 
            : base(ClassReportType.CrfClass, MainFileReservedNames.FinancialCrfClass, ReportConstants.CrfClass) { }

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
