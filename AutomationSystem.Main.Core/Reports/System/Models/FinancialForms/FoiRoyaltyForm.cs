using System.Collections.Generic;
using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.FinancialForms
{
    /// <summary>
    /// Foi Royalty model template
    /// </summary>
    public class FoiRoyaltyForm
    {

        [SheetGroup("GeneralInfo")]
        public GeneralInfo GeneralInfo { get; set; }

        [SheetGroup("ReimbursementForPrinting")]
        public ReimbursementForPrinting ReimbursementForPrinting { get; set; }

        [SheetTable("RoyaltyFeeItems")]
        public List<RoyaltyFeeItems> RoyaltyFeeItems { get; set; }

        public FoiRoyaltyForm()
        {
            GeneralInfo = new GeneralInfo();
            ReimbursementForPrinting = new ReimbursementForPrinting();
            RoyaltyFeeItems = new List<RoyaltyFeeItems>();
        }

    }

}
