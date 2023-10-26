using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances
{
    /// <summary>
    /// Form for editing layout of class expenses
    /// </summary>
    public class ExpensesLayoutForm
    {
        [HiddenInput]
        public long EntityId { get; set; }

        [DisplayName("Is enabled")]
        public int[] EnabledExpenses { get; set; } = new int[0];

        [DisplayName("Class expenses")]
        public List<ExpenseForm> Expenses { get; set; } = new List<ExpenseForm>();
    }
}