using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    /// <summary>
    /// Fa program expanse
    /// </summary>
    public class FaProgramExpenseItem
    {

        // program expance
        [SheetField("Text")]
        public string Text { get; set; }

        [SheetField("Value")]
        public decimal Value { get; set; }        

    }

}
