using System.Collections.Generic;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;

namespace AutomationSystem.Main.Core.Reports.System.ReportAvailabilityResolvers
{
    /// <summary>
    /// Resolves which reports are available for given class
    /// </summary>
    public interface IReportAvailabilityResolver
    {
        bool IsReportTypeAvailable(ClassReportType type);

        HashSet<ClassReportType> GetAvailableReportTypes();

        HashSet<ClassReportType> GetReportTypesForMasterCoordinatorEmail();

        HashSet<ClassReportType> GetReportTypesForDailyReports();

        HashSet<ClassReportType> GetReportTypesForFinalReports();
    }
}