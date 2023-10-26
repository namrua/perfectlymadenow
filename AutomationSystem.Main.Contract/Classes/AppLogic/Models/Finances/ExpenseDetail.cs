using System.ComponentModel;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances
{
    /// <summary>
    /// Class expense detail
    /// </summary>
    public class ExpenseDetail
    {
        [DisplayName("Order No.")]
        public int Order { get; set; }

        [DisplayName("Text")]
        public string Text { get; set; }

        [DisplayName("Expense type code")]
        public ClassExpenseTypeEnum ClassExpenseTypeId { get; set; }

        [DisplayName("Expense type")]
        public string ClassExpenseType { get; set; }

        [DisplayName("Value")]
        public decimal? Value { get; set; }
    }
}