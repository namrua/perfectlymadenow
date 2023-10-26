using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    public class FaProgramRevenueItem
    {
        [SheetField("Quantity")]
        public int Quantity { get; set; }

        [SheetField("Rate")]
        public decimal Rate { get; set; }
    }
}
