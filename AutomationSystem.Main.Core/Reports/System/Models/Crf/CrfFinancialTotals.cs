using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.Crf
{
    /// <summary>
    /// Financial totals info
    /// </summary>
    public class CrfFinancialTotals
    {
        [SheetField("TotalRevenue")]
        public decimal TotalRevenue { get; set; }

        [SheetField("TotalApproved")]
        public decimal? TotalApproved { get; set; }
    }
}
