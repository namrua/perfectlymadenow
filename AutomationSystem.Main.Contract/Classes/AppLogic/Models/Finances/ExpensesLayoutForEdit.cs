using System.Collections.Generic;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances
{
    /// <summary>
    /// Class expenses layout for edit
    /// </summary>
    public class ExpensesLayoutForEdit
    {
        public List<ClassExpenseType> ExpenseTypes { get; set; } = new List<ClassExpenseType>();
        public ExpensesLayoutForm Form { get; set; } = new ExpensesLayoutForm();
        public string CurrencyCode { get; set; }
    }
}
