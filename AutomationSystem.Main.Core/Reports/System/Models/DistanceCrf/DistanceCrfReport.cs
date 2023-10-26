using System.Collections.Generic;
using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.DistanceCrf
{
    /// <summary>
    /// Model for distance CRF report
    /// </summary>
    public class DistanceCrfReport 
    {
        [SheetGroup("Header")]
        public DistanceCrfHeader Header { get; set; } = new DistanceCrfHeader();

        [SheetGroup("Counts")]
        public DistanceCrfCounts Counts { get; set; } = new DistanceCrfCounts();

        [SheetTable("Students")]
        public List<DistanceCrfStudent> Students { get; set; } = new List<DistanceCrfStudent>();
    }
}