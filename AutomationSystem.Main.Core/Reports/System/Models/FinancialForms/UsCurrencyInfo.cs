using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    // US currency info
    public class UsCurrencyInfo
    {

        [SheetField("ClassRevenue")]
        public decimal ClassRevenue { get; set; }

        [SheetField("PaidStudentsCount")]
        public int PaidStudentsCount { get; set; }

        [SheetField("GuestInstructorFee")]
        public decimal GuestInstructorFee { get; set; }

    }

}
