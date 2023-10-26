using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.DistanceCrf
{
    /// <summary>
    /// Encapsulates student's counts
    /// </summary>
    public class DistanceCrfCounts
    {
        [SheetField("Absentee")]
        public int Absentee { get; set; }
    }
}