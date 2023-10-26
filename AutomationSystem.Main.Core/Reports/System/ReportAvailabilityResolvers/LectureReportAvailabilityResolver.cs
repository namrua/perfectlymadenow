using System.Collections.Generic;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;

namespace AutomationSystem.Main.Core.Reports.System.ReportAvailabilityResolvers
{
    /// <summary>
    /// Determines which reports are available for specified class (with class type category = lecture)
    /// </summary>
    public class LectureReportAvailabilityResolver : IReportAvailabilityResolver
    {
        private readonly HashSet<ClassReportType> availableReportTypes;

        public LectureReportAvailabilityResolver()
        {
            availableReportTypes = new HashSet<ClassReportType>(new [] { ClassReportType.CrfLecture, ClassReportType.CountriesReport });
        }

        public bool IsReportTypeAvailable(ClassReportType type)
        {
            return availableReportTypes.Contains(type);
        }

        public HashSet<ClassReportType> GetAvailableReportTypes()
        {
            return new HashSet<ClassReportType>(availableReportTypes);
        }
        
        public HashSet<ClassReportType> GetReportTypesForMasterCoordinatorEmail()
        {
            return new HashSet<ClassReportType>();
        }     

        public HashSet<ClassReportType> GetReportTypesForDailyReports()
        {
            return new HashSet<ClassReportType>(availableReportTypes);
        }

        public HashSet<ClassReportType> GetReportTypesForFinalReports()
        {
            return new HashSet<ClassReportType>(availableReportTypes);
        }
    }
}
