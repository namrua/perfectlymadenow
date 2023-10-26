using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Model;
using CorabeuControl.Components;
using System;
using System.Linq;

namespace AutomationSystem.Main.Core.Classes.System.Convertors
{

    /// <summary>
    /// Class business detail convertor
    /// </summary>
    public class ClassBusinessConvertor : IClassBusinessConvertor
    {

        private readonly IClassDatabaseLayer classDb;
        private readonly IMainMapper mainMapper;
        private readonly IClassTypeResolver classTypeResolver;



        // constructor
        public ClassBusinessConvertor(IClassDatabaseLayer classDb, IMainMapper mainMapper, IClassTypeResolver classTypeResolver)
        {
            this.classDb = classDb;
            this.mainMapper = mainMapper;
            this.classTypeResolver = classTypeResolver;
        }


        // creates class business entity by class preference
        public ClassBusiness CreateClassBusinessByClassPreference(Class cls, ClassPreference preference)
        {
            var result = new ClassBusiness();
            if (!classTypeResolver.AreFinancialFormsAllowed(cls.ClassCategoryId))
                return result;

            // adds expenses
            var expenses = preference.ClassPreferenceExpenses.Select(mainMapper.Map<ClassExpense>).ToList();
            result.ClassExpenses = expenses;
            return result;
        }


        // initializes ClassBusinessForEdit 
        public ClassBusinessForEdit InitializeClassBusinessForEdit(Class cls)
        {
            if (cls.Currency == null)
            {
                throw new ArgumentException("Currency is not included into Class object.");
            }
            var result = new ClassBusinessForEdit();
            result.AreFinancialFormsAllowed = classTypeResolver.AreFinancialFormsAllowed(cls.ClassCategoryId);
            if (!result.AreFinancialFormsAllowed) return result;
            result.CurrencyCode = cls.Currency.Name;

            // loads candidates for associated lecture
            var classUtcStartTime = cls.EventStartUtc;
            var filter = new ClassFilter
            {
                ProfileId = cls.ProfileId,
                OpenAndCompleted = true,
                EventEndUtcFrom = classUtcStartTime.AddMonths(-1),
                EventEndUtcTo = classUtcStartTime.AddDays(1),
                ClassCategoryId = ClassCategoryEnum.Lecture,
                CurrencyId = cls.CurrencyId
            };
            var candidatesForLecture = classDb.GetClassesByFilter(filter, ClassIncludes.ClassType);
            result.Lectures = candidatesForLecture
                .OrderBy(x => x.EventStartUtc)
                .Select(x => DropDownItem.Item(x.ClassId, ClassConvertor.GetClassTitle(x))).ToList();
            return result;
        }


        // converts ClassBusiness to ClassBusinessForm
        public ClassBusinessForm ConvertToClassBusinessForm(ClassBusiness business, long classId)
        {
            var result = new ClassBusinessForm
            {
                ClassId = classId,               
                ApprovedBudget = business.ApprovedBudget,
                PrintReimbursement = business.PrintReimbursement,
                AssociatedLectureId = business.AssociatedLectureId,
                CustomExpenses = business.ClassExpenses
                    .Where(x => x.ClassExpenseTypeId == ClassExpenseTypeEnum.Custom)
                    .Select(ConvertToClassCustomExpensesForm)
                    .OrderBy(x => x.Order).ToList()
            };
            return result;
        }


        // converts ClassBusiness to ClassBusinessDetail
        public ClassBusinessDetail ConvertToClassBusinesDetail(ClassBusiness business, Class cls)
        {
            if (cls.Currency == null)
            {
                throw new ArgumentException("Currency is not included into Class object.");
            }
            if (business.ClassExpenses == null)
                throw new InvalidOperationException("ClassExpenses is not included into ClassBusiness object.");

            var result = new ClassBusinessDetail
            {
                AreFinancialFormsAllowed = classTypeResolver.AreFinancialFormsAllowed(cls.ClassCategoryId),
                ApprovedBudget = business.ApprovedBudget,
                PrintReimbursement = business.PrintReimbursement,
                AssociatedLectureId = business.AssociatedLectureId,
                Expenses = business.ClassExpenses.Select(mainMapper.Map<ExpenseDetail>).ToList(),
                CurrencyCode = cls.Currency.Name
            };

            // gets associated lecture title
            if (result.AssociatedLectureId.HasValue)
            {
                var associatedLecture = classDb.GetClassById(result.AssociatedLectureId.Value, ClassIncludes.ClassType);
                if (associatedLecture == null)
                    throw new ArgumentException($"There is no lecture with id {result.AssociatedLectureId.Value}.");
                result.AccociatedLecture = ClassConvertor.GetClassTitle(associatedLecture);
            }
            return result;
        }       


        // converts ClassBusinessForm to ClassBusiness
        public ClassBusiness ConvertToClassBusines(ClassBusinessForm form, Class cls)
        {
            var result = new ClassBusiness
            {               
                ApprovedBudget = form.ApprovedBudget,
                PrintReimbursement = form.PrintReimbursement,
                AssociatedLectureId = form.AssociatedLectureId               
            };
            if (!classTypeResolver.AreFinancialFormsAllowed(cls.ClassCategoryId))
                return result;

            // converts custom expenses
            result.ClassExpenses = form.CustomExpenses.Select(ConvertToClassExpense).ToList();
            return result;
        }


        #region private methods - custom expense form convertors

        // converts ClassExpense to ClassCustomExpenseForm
        private ClassCustomExpenseForm ConvertToClassCustomExpensesForm(ClassExpense classExpense)
        {
            var result = new ClassCustomExpenseForm
            {
                Order = classExpense.Order,
                Text = classExpense.Text,
                Value = classExpense.Value
            };
            return result;
        }


        // converts ClassCustomExpenseForm to ClassExpense
        private ClassExpense ConvertToClassExpense(ClassCustomExpenseForm form)
        {
            var result = new ClassExpense
            {
                Order = form.Order,
                Text = form.Text,
                ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                Value = form.Value
            };
            return result;
        }

        #endregion

    }

}
