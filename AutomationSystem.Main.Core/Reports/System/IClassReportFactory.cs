using System.Collections.Generic;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;
using AutomationSystem.Main.Core.Reports.System.ReportGenerators;

namespace AutomationSystem.Main.Core.Reports.System
{
    /// <summary>
    /// Creates class report components
    /// </summary>
    public interface IClassReportFactory
    {
        IClassReportComponents InitializeClassReportComponentsForClass(long classId);

        IClassReportGenerator GetClassReportGeneratorByReportType(ClassReportType reportType);

        List<IClassReportGenerator> GetClassReportGeneratorsByReportTypes(IEnumerable<ClassReportType> reportTypes);
    }
}