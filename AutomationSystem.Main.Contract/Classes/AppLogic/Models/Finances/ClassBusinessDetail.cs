using System.Collections.Generic;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances
{
    /// <summary>
    /// Class business detail
    /// </summary>
    public class ClassBusinessDetail
    {
        [DisplayName("Are Financial Forms Allowed")]
        public bool AreFinancialFormsAllowed { get; set; }

        [DisplayName("Approved budget")]
        public decimal? ApprovedBudget { get; set; }

        [DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        [DisplayName("Reimbursement for printing")]
        public decimal? PrintReimbursement { get; set; }

        [DisplayName("Associated lecture ID")]
        public long? AssociatedLectureId { get; set; }

        [DisplayName("Accociated lecture")]
        public string AccociatedLecture { get; set; }

        [DisplayName("Expenses")]
        public List<ExpenseDetail> Expenses { get; set; } = new List<ExpenseDetail>();
    }
}