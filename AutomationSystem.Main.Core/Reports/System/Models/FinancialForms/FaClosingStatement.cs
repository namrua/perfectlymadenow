using System.Collections.Generic;
using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    /// <summary>
    /// Fa closing statement model template
    /// </summary>
    public class FaClosingStatement
    {

        [SheetGroup("GeneralInfo")]
        public GeneralInfo GeneralInfo { get; set; }

        [SheetGroup("CrfDetail")]
        public FaCrfDetail CrfDetail { get; set; }

        [SheetTable("ProgramExpenses")]
        public List<FaProgramExpenseItem> ProgramExpenses { get; set; }

        [SheetTable("ProgramRevenueItems")]
        public List<FaProgramRevenueItem> ProgramRevenueItems { get; set; }


        // constructor
        public FaClosingStatement()
        {
            GeneralInfo = new GeneralInfo();
            CrfDetail = new FaCrfDetail();
            ProgramExpenses = new List<FaProgramExpenseItem>();
            ProgramRevenueItems = new List<FaProgramRevenueItem>();
        }

    }
}
