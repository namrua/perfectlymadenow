using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    /// <summary>
    /// Revenue deposit form model template
    /// </summary>
    public class RevenueDepositForm
    {

        [SheetGroup("GeneralInfo")]
        public GeneralInfo GeneralInfo { get; set; }

        [SheetGroup("DepositCalculations")]
        public DepositCalculations DepositCalculations { get; set; }

        // constructor
        public RevenueDepositForm()
        {
            GeneralInfo = new GeneralInfo();
            DepositCalculations = new DepositCalculations();
        }

    }

}
