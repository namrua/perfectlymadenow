using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    // Guest instructor statement model template
    public class GuestInstructorStatement
    {

        // public properties
        [SheetGroup("GeneralInfo")]
        public GeneralInfo GeneralInfo { get; set; }

        [SheetGroup("USCurrencyInfo")]
        public UsCurrencyInfo USCurrencyInfo { get; set; }

        // constructor
        public GuestInstructorStatement()
        {
            GeneralInfo = new GeneralInfo();
            USCurrencyInfo = new UsCurrencyInfo();
        }

    }

}
