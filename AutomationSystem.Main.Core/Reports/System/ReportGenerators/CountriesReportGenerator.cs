using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;

namespace AutomationSystem.Main.Core.Reports.System.ReportGenerators
{
    /// <summary>
    /// Countries report generator
    /// </summary>
    public class CountriesReportGenerator : BaseWordReportGenerator
    {
        public CountriesReportGenerator()
            : base(ClassReportType.CountriesReport, MainFileReservedNames.CountriesReport, ReportConstants.CountriesReport)
        {
        }

        public override byte[] GenerateReport(IClassReportComponents components, string rootPath, string reportFolder)
        {            
            var model = components.Common.GetCountriesReport();
            var content = wordDocumentCreator.GetCountriesReport(model);
            return content;
        }
    }
}
