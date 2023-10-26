using System.Collections.Generic;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;

namespace AutomationSystem.Main.Core.Reports.System.ReportAvailabilityResolvers
{
    /// <summary>
    /// Determines which reports are available for specified class (with class type category = unknown)
    /// </summary>
    public class EmptyReportAvailabilityResolver : IReportAvailabilityResolver
    {
        public bool IsReportTypeAvailable(ClassReportType type)
        {
            return false;
        }

        public HashSet<ClassReportType> GetAvailableReportTypes()
        {
            return new HashSet<ClassReportType>();
        }

        public HashSet<ClassReportType> GetReportTypesForMasterCoordinatorEmail()
        {
            return new HashSet<ClassReportType>();
        }

        public HashSet<ClassReportType> GetReportTypesForDailyReports()
        {
            return new HashSet<ClassReportType>();
        }

        public HashSet<ClassReportType> GetReportTypesForFinalReports()
        {
            return new HashSet<ClassReportType>();
        }
    }
}
