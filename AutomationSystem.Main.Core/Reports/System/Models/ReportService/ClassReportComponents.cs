using System;
using AutomationSystem.Main.Core.Reports.System.DataProviders;
using AutomationSystem.Main.Core.Reports.System.ReportAvailabilityResolvers;

namespace AutomationSystem.Main.Core.Reports.System.Models.ReportService
{
    /// <summary>
    /// Encapsulates class report components
    /// </summary>
    public class ClassReportComponents : IClassReportComponents
    {
        private readonly IFinancialFormsDataProvider financial;

        public IClassDataProvider Data { get; }
        public ICommonReportDataProvider Common { get; }
        public IFinancialFormsDataProvider Financial => financial ?? throw new InvalidOperationException("FinancialFormsDataProvider is not available for the class");
        public IReportAvailabilityResolver AvailabilityResolver { get; }

        public ClassReportComponents(IClassDataProvider data, ICommonReportDataProvider common,
            IFinancialFormsDataProvider financial, IReportAvailabilityResolver availabilityResolver)
        {
            Data = data;
            Common = common;
            this.financial = financial;
            AvailabilityResolver = availabilityResolver;
        }
    }
}
