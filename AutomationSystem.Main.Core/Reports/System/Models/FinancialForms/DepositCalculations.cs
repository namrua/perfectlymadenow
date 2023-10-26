using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{

    /// <summary>
    /// Deposit calculations
    /// </summary>
    public class DepositCalculations
    {

        [SheetField("PaidStudentsCount")]
        public int PaidStudentsCount { get; set; }

        [SheetField("TotalRevennue")]
        public decimal TotalRevennue { get; set; }

        [SheetField("ApprovedBudget")]
        public decimal? ApprovedBudget { get; set; }

    }

}
