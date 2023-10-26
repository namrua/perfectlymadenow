using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    /// <summary>
    /// Fa guest instructor fee
    /// </summary>
    public class FaCrfDetail
    {

        [SheetField("PaidStudentsCount")]
        public int PaidStudentsCount { get; set; }

        [SheetField("ProgramRevenue")]
        public decimal? ProgramRevenue { get; set; }

    }

}
