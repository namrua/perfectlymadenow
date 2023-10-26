using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Factories
{
    /// <summary>
    /// Creates ClassExpense related objects
    /// </summary>
    public interface IClassExpenseFactory
    {
        ExpensesLayoutForEdit CreateExpensesLayoutForEdit(Currency currency);
    }
}
