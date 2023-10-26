using System;
using System.Collections.Generic;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.Reports.System.DataProviders;

namespace AutomationSystem.Main.Core.Reports.System.ReportAvailabilityResolvers
{
    /// <summary>
    /// Determines which reports are available for specified class (with class type category = class)
    /// </summary>
    public class ClassReportAvailabilityResolver : IReportAvailabilityResolver
    {
        private readonly IClassDataProvider data;
        private readonly IClassFinancialBusinessLogic logic;

        private readonly Lazy<HashSet<ClassReportType>> availableReportTypes;

        public ClassReportAvailabilityResolver(IClassDataProvider data, IClassFinancialBusinessLogic logic)
        {
            this.data = data;
            this.logic = logic;

            availableReportTypes = new Lazy<HashSet<ClassReportType>>(ComputeAvailableReportTypes);
        }

        public bool IsReportTypeAvailable(ClassReportType type)
        {
            return availableReportTypes.Value.Contains(type);
        }

        public HashSet<ClassReportType> GetAvailableReportTypes()
        {
            return availableReportTypes.Value;
        }

        public HashSet<ClassReportType> GetReportTypesForMasterCoordinatorEmail()
        {
            var result = new HashSet<ClassReportType>(new[] { ClassReportType.CrfClass });
            result.IntersectWith(availableReportTypes.Value);
            return result;
        }

        public HashSet<ClassReportType> GetReportTypesForDailyReports()
        {
            var result = new HashSet<ClassReportType>(new[] { ClassReportType.CrfClass, ClassReportType.CrfWwaClass });
            result.IntersectWith(availableReportTypes.Value);
            return result;
        }

        public HashSet<ClassReportType> GetReportTypesForFinalReports()
        {
            var result = new HashSet<ClassReportType>(new[] { ClassReportType.CrfClass, ClassReportType.CrfWwaClass, ClassReportType.CountriesReport,
                ClassReportType.FoiRoyaltyForm, ClassReportType.FaClosingStatement, ClassReportType.GuestInstructorClosingStatement });
            result.IntersectWith(availableReportTypes.Value);
            return result;            
        }

        #region private fiedls

        private HashSet<ClassReportType> ComputeAvailableReportTypes()
        {
            var result = new HashSet<ClassReportType>();
            result.Add(ClassReportType.CrfClass);
            if (data.Class.IsWwaFormAllowed)
                result.Add(ClassReportType.CrfWwaClass);
            
            result.Add(ClassReportType.FoiRoyaltyForm);
            result.Add(ClassReportType.FaClosingStatement);                       
            if (logic.IsGuestInstructorRevenueAvailable)
                result.Add(ClassReportType.GuestInstructorClosingStatement);

            result.Add(ClassReportType.CountriesReport);
            return result;
        }

        #endregion
    }
}
