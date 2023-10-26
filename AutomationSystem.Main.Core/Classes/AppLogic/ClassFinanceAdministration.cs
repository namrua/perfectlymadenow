using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Contract.Classes.AppLogic;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Model;
using System;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Classes.AppLogic
{
    public class ClassFinanceAdministration : IClassFinanceAdministration
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IClassExpenseFactory classExpenseFactory;
        private readonly IMainMapper mainMapper;
        private readonly IClassTypeResolver classTypeResolver;

        private readonly IClassBusinessConvertor classBusinessConvertor;

        // constructor
        public ClassFinanceAdministration(
            IClassDatabaseLayer classDb,
            IIdentityResolver identityResolver,
            IClassExpenseFactory classExpenseFactory,
            IMainMapper mainMapper,
            IClassBusinessConvertor classBusinessConvertor,
            IClassTypeResolver classTypeResolver)
        {
            this.classDb = classDb;
            this.identityResolver = identityResolver;
            this.classExpenseFactory = classExpenseFactory;
            this.mainMapper = mainMapper;
            this.classBusinessConvertor = classBusinessConvertor;
            this.classTypeResolver = classTypeResolver;
        }

        public ClassFinancePageModel GetClassFinancePageModel(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassType | ClassIncludes.ClassBusinessClassExpenses | ClassIncludes.Currency);
            identityResolver.CheckEntitleForClass(cls);

            var result = new ClassFinancePageModel();
            result.Class = ClassConvertor.ConvertToClassShortDetial(cls);

            // resolves availability of finances
            if (!classTypeResolver.AreFinancialFormsAllowed(cls.ClassCategoryId))
            {
                result.AreFinanceDisabled = true;
                result.FinanceDisabledMessage = "Finance are not available for the class.";
                return result;
            }

            result.Finance = classBusinessConvertor.ConvertToClassBusinesDetail(cls.ClassBusiness, cls);
            return result;
        }

        public ClassBusinessForEdit GetClassBusinessForEditByClassId(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassBusinessClassExpenses | ClassIncludes.Currency);
            identityResolver.CheckEntitleForClass(cls);

            var result = classBusinessConvertor.InitializeClassBusinessForEdit(cls);
            result.Form = classBusinessConvertor.ConvertToClassBusinessForm(cls.ClassBusiness, classId);
            return result;
        }

        public ClassBusinessForEdit GetFormClassBusinessForEdit(ClassBusinessForm form)
        {
            var cls = GetClassById(form.ClassId, ClassIncludes.ClassBusinessClassExpenses);
            identityResolver.CheckEntitleForClass(cls);

            var result = classBusinessConvertor.InitializeClassBusinessForEdit(cls);
            result.Form = form;
            return result;
        }
        
        public void SaveClassBusiness(ClassBusinessForm form)
        {
            var cls = GetClassById(form.ClassId, ClassIncludes.ClassBusinessClassExpenses);
            identityResolver.CheckEntitleForClass(cls);

            var toUpdate = classBusinessConvertor.ConvertToClassBusines(form, cls);
            classDb.UpdateClassBusinessByClassId(form.ClassId, toUpdate, true);            
        }

        public ExpensesLayoutForEdit GetExpensesLayoutForEditByClassId(long classId)
        {
            var cls = GetClassById(classId, ClassIncludes.ClassBusinessClassExpenses | ClassIncludes.Currency);
            identityResolver.CheckEntitleForClass(cls);

            var result = classExpenseFactory.CreateExpensesLayoutForEdit(cls.Currency);
            result.Form = mainMapper.Map<ExpensesLayoutForm>(cls.ClassBusiness.ClassExpenses);
            result.Form.EntityId = classId;
            return result;
        }

        public ExpensesLayoutForEdit GetFormExpensesLayoutForEdit(ExpensesLayoutForm form)
        {
            var cls = GetClassById(form.EntityId, ClassIncludes.Currency);
            identityResolver.CheckEntitleForClass(cls);

            var result = classExpenseFactory.CreateExpensesLayoutForEdit(cls.Currency);
            result.Form = form;
            return result;
        }

        public void SaveExpensesLayout(ExpensesLayoutForm form)
        {
            var toCheck = GetClassById(form.EntityId);
            identityResolver.CheckEntitleForClass(toCheck);
            
            var expensesToUpdate = mainMapper.Map<List<ClassExpense>>(form);
            classDb.UpdateClassExpenses(form.EntityId, expensesToUpdate);
        }

        #region private methods

        public Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }

            return result;
        }

        #endregion
    }
}