using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;

namespace AutomationSystem.Main.Contract.Classes.AppLogic
{
    public interface IClassFinanceAdministration
    {
        ClassFinancePageModel GetClassFinancePageModel(long classId);

        ClassBusinessForEdit GetClassBusinessForEditByClassId(long classId);

        ClassBusinessForEdit GetFormClassBusinessForEdit(ClassBusinessForm form);

        void SaveClassBusiness(ClassBusinessForm form);

        ExpensesLayoutForEdit GetExpensesLayoutForEditByClassId(long classId);

        ExpensesLayoutForEdit GetFormExpensesLayoutForEdit(ExpensesLayoutForm form);

        void SaveExpensesLayout(ExpensesLayoutForm form);
    }
}
