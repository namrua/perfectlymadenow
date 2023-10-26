using System;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Reports.System.Models.DistanceReportService
{
    /// <summary>
    /// Determines Distance CRF report parameters
    /// </summary>
    public class DistanceCrfReportParameters
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long DistanceCoordinatorId { get; set; }
        public List<long> ProfileIds { get; set; }              // null - all profiles
    }
}