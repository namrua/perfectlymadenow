using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    /// <summary>
    /// Print reimbursement data
    /// </summary>
    public class ReimbursementForPrinting
    {

        [SheetField("Rate")]
        public decimal Rate { get; set; }

        [SheetField("Number")]
        public int Number { get; set; } 

    }

}
