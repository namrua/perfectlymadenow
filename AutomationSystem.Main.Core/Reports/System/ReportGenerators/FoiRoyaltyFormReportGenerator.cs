using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Foi Ryalty Form report generator
    /// </summary>
    public class FoiRoyaltyFormReportGenerator : BaseExcelReportGenerator
    {
        private const string reportDefinition = "FoiRoyaltyForm.xml";

        public FoiRoyaltyFormReportGenerator()
            : base(ClassReportType.FoiRoyaltyForm, MainFileReservedNames.FoiRoyaltyForm, ReportConstants.FoiRoyaltyForm)
        {
        }

        public override byte[] GenerateReport(IClassReportComponents components, string rootPath, string reportFolder)
        {
            // creating 
            var model = components.Financial.GetFoiRoyaltyFormModel();
            var writer = GetSheetCreator(rootPath, reportFolder, reportDefinition);
            var content = writer.CreateSheet(model);
            return content;
        }
    }
}
