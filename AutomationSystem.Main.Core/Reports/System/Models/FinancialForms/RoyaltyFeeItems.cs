using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    public class RoyaltyFeeItems
    {
        [SheetField("Rate")]
        public decimal Rate { get; set; }

        [SheetField("Number")]
        public int Number { get; set; }
    }
}
