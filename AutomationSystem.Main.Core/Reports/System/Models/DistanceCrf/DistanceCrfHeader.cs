using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.DistanceCrf
{
    /// <summary>
    /// Distance CRF header
    /// </summary>
    public class DistanceCrfHeader
    {
        [SheetField("Coordinator")]
        public string Coordinator { get; set; }

        [SheetField("CoordinatorNo")]
        public int CoordinatorNo { get; set; }

        [SheetField("StartDate")]
        public string StartDate { get; set; }

        [SheetField("EndDate")]
        public string EndDate { get; set; }
    }
}