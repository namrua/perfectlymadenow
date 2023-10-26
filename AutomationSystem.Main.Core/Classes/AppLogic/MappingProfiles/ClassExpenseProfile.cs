using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Mapping;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.Classes.AppLogic.MappingProfiles
{
    /// <summary>
    /// Maps ClassExpense related objects
    /// </summary>
    public class ClassExpenseProfile : Profile
    {
        public ClassExpenseProfile(IClassDatabaseLayer classDb)
        {
            var classExpenseTypeConverter = new TypeConverterMapper<ClassExpenseType, ClassExpenseTypeEnum, string>(
                classDb.GetClassExpenseTypes, x => x.ClassExpenseTypeId, x => x.Description);

            CreateMap<ClassExpenseTypeEnum, string>().ConvertUsing(classExpenseTypeConverter);
            CreateMap<ClassPreferenceExpense, ClassExpense>()
                .ForMember(dest => dest.ClassExpenseType, opt => opt.Ignore());
            CreateMap<ClassPreferenceExpense, ExpenseDetail>()
                .ForMember(dest => dest.ClassExpenseType, opt => opt.MapFrom(src => src.ClassExpenseTypeId));
            CreateMap<ClassExpense, ExpenseDetail>()
                .ForMember(dest => dest.ClassExpenseType, opt => opt.MapFrom(src => src.ClassExpenseTypeId));

            CreateMap<ExpenseForm, ClassPreferenceExpense>();
            CreateMap<ExpensesLayoutForm, List<ClassPreferenceExpense>>()
                .ForCtorParam("collection", opt => opt.MapFrom((src, context) => context.Mapper.Map<List<ExpenseForm>>(src)));

            CreateMap<ExpensesLayoutForm, List<ExpenseForm>>()
                .ForCtorParam("collection", opt => opt.MapFrom(src => MapExpenseLayoutFormToExpenseForm(src)));
            CreateMap<ClassPreferenceExpense, ExpenseForm>();
            CreateMap<List<ExpenseForm>, ExpensesLayoutForm>()
                .ForMember(dest => dest.EnabledExpenses, opt => opt.MapFrom(src => src.Select(x => x.Order).ToArray()))
                .ForMember(dest => dest.Expenses, opt => opt.MapFrom(src => CombineWithDefaultExpenses(src)));
            CreateMap<List<ClassPreferenceExpense>, ExpensesLayoutForm>()
                .AfterMap((src, dest, context) => context.Mapper.Map(context.Mapper.Map<List<ExpenseForm>>(src), dest));
            CreateMap<ExpenseForm, ClassExpense>();
            CreateMap<ExpensesLayoutForm, List<ClassExpense>>()
                .ForCtorParam("collection", opt => opt.MapFrom(src => MapExpenseLayoutFormToExpenseForm(src)));
            CreateMap<ClassExpense, ExpenseForm>();
            CreateMap<List<ClassExpense>, ExpensesLayoutForm>()
                .AfterMap((src, dest, context) => context.Mapper.Map(context.Mapper.Map<List<ExpenseForm>>(src), dest));
        }

        private List<ExpenseForm> MapExpenseLayoutFormToExpenseForm(ExpensesLayoutForm form)
        {
            var enabledExpenses = new HashSet<int>(form.EnabledExpenses);
            var result = form.Expenses
                .Where(x => enabledExpenses.Contains(x.Order)).ToList();
            return result;
        }

        private List<ExpenseForm> CombineWithDefaultExpenses(List<ExpenseForm> formClassExpenses)
        {
            var expenseMap = formClassExpenses.ToDictionary(x => x.Order);
            var result = new List<ExpenseForm>();
            for (var i = 0; i < FinancialFormConstants.OriginalExpenses.Length; i++)
            {
                var order = i + 1;

                if (!expenseMap.TryGetValue(order, out var item))
                {
                    item = new ExpenseForm
                    {
                        Order = order,
                        Text = FinancialFormConstants.OriginalExpenses[i],
                        ClassExpenseTypeId = ClassExpenseTypeEnum.Custom
                    };
                }
                result.Add(item);
            }
            return result;
        }
    }
}
