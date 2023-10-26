using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Factories
{
    /// <summary>
    /// Creates profile related objects
    /// </summary>
    public class ClassExpenseFactory : IClassExpenseFactory
    {
        private readonly IClassDatabaseLayer classDb;

        public ClassExpenseFactory(IClassDatabaseLayer classDb)
        {
            this.classDb = classDb;
        }

        public ExpensesLayoutForEdit CreateExpensesLayoutForEdit(Currency currency)
        {
            var result = new ExpensesLayoutForEdit();
            result.ExpenseTypes = classDb.GetClassExpenseTypes();
            result.CurrencyCode = currency.Name;
            return result;
        }
    }
}
