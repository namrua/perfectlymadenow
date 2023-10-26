using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Finances;
using AutomationSystem.Main.Core.Classes.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Reports.System.Models.ReportService;
using AutomationSystem.Main.Model;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.MappingProfiles
{
    public class ClassExpenseProfileTests
    {
        private readonly Mock<IClassDatabaseLayer> classDbMock;

        public ClassExpenseProfileTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
            AddClassExpenseTypes();
        }

        #region CreateMap<ClassExpenseTypeEnum, string>() tests
        [Theory]
        [InlineData(ClassExpenseTypeEnum.Custom, "Custom")]
        [InlineData(ClassExpenseTypeEnum.FoiRoyaltyFee, "FOI Royalty Fee")]
        [InlineData(ClassExpenseTypeEnum.PayPalFeeWwaLecture, null)]
        public void Map_ClassExpenseTypeEnum_ReturnsExpectedString(ClassExpenseTypeEnum expenseType, string expectedValue)
        {
            // arrange
            var mapper = CreateMapper();

            // act
            var actualValue = mapper.Map<string>(expenseType);

            // assert
            Assert.Equal(expectedValue, actualValue);
        }
        #endregion

        #region CreateMap<ClassPreferenceExpense, ClassExpense>() tests
        [Fact]
        public void Map_ClassPreferenceExpense_ReturnsClassExpense()
        {
            // arrange
            var classPreferenceExpense = new ClassPreferenceExpense
            {
                Order = 1,
                Text = "Text",
                ClassExpenseTypeId = ClassExpenseTypeEnum.FoiRoyaltyFee,
                ClassExpenseType = new ClassExpenseType(),
                Value = 1
            };
            var mapper = CreateMapper();

            // act
            var classExpense = mapper.Map<ClassExpense>(classPreferenceExpense);

            // assert
            Assert.Equal(1, classExpense.Order);
            Assert.Equal("Text", classExpense.Text);
            Assert.Equal(ClassExpenseTypeEnum.FoiRoyaltyFee, classExpense.ClassExpenseTypeId);
            Assert.Equal(1, classExpense.Value);
            Assert.Null(classExpense.ClassExpenseType);
        }
        #endregion

        #region CreateMap<ClassPreferenceExpense, ExpenseDetail>() tests
        [Fact]
        public void Map_ClassPreferenceExpense_ReturnsExpenseDetail()
        {
            // arrange
            var classPreferenceExpense = new ClassPreferenceExpense
            {
                Order = 1,
                Text = "Text",
                ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                Value = 1
            };
            var mapper = CreateMapper();

            // act
            var expanseDetail = mapper.Map<ExpenseDetail>(classPreferenceExpense);

            // assert
            Assert.Equal(1, expanseDetail.Order);
            Assert.Equal("Text", expanseDetail.Text);
            Assert.Equal(ClassExpenseTypeEnum.Custom, expanseDetail.ClassExpenseTypeId);
            Assert.Equal(1, expanseDetail.Value);
            Assert.Equal("Custom", expanseDetail.ClassExpenseType);
        }
        #endregion

        #region CreateMap<ClassExpense, ExpenseDetail>() tests
        [Fact]
        public void Map_ClassExpense_ReturnsExpenseDetail()
        {
            // arrange
            var classExpense = new ClassExpense
            {
                Order = 1,
                Text = "Text",
                ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                Value = 1
            };
            var mapper = CreateMapper();

            // act
            var expanseDetail = mapper.Map<ExpenseDetail>(classExpense);

            // assert
            Assert.Equal(1, expanseDetail.Order);
            Assert.Equal("Text", expanseDetail.Text);
            Assert.Equal(ClassExpenseTypeEnum.Custom, expanseDetail.ClassExpenseTypeId);
            Assert.Equal(1, expanseDetail.Value);
            Assert.Equal("Custom", expanseDetail.ClassExpenseType);
        }
        #endregion

        #region CreateMap<ExpenseForm, ClassPreferenceExpense>() tests
        [Fact]
        public void Map_ExpenseForm_ReturnsClassPreferenceExpense()
        {
            // arrange
            var form = new ExpenseForm
            {
                Order = 1,
                Text = "Expense",
                ClassExpenseTypeId = ClassExpenseTypeEnum.FoiRoyaltyFee,
                Value = 1.5M
            };
            var mapper = CreateMapper();

            // act
            var prefExpense = mapper.Map<ClassPreferenceExpense>(form);

            // assert
            Assert.Equal(1, prefExpense.Order);
            Assert.Equal("Expense", prefExpense.Text);
            Assert.Equal(ClassExpenseTypeEnum.FoiRoyaltyFee, prefExpense.ClassExpenseTypeId);
            Assert.Equal(1.5M, prefExpense.Value);
        }
        #endregion

        #region CreateMap<ExpensesLayoutForm, List<ClassPreferenceExpense>>() tests
        [Fact]
        public void Map_ExpensesLayoutForm_ReturnsClassPreferenceExpenses()
        {
            // arrange
            var form = new ExpensesLayoutForm
            {
                EntityId = 1,
                EnabledExpenses = new[] { 1, 3 },
                Expenses = CreateExpanseForms()
            };
            var mapper = CreateMapper();

            // act
            var classPreferenceExpenses = mapper.Map<List<ClassPreferenceExpense>>(form);

            // assert
            Assert.Collection(classPreferenceExpenses,
                item => Assert.Equal(1, item.Order),
                item => Assert.Equal(3, item.Order));
        }
        #endregion

        #region CreateMap<ExpenseLayoutForm, List<ExpenseForm>>() tests
        [Fact]
        public void Map_ExpensesLayoutForm_ReturnsExpenseForms()
        {
            // arrange
            var expenseLayoutForm = new ExpensesLayoutForm
            {
                EntityId = 1,
                EnabledExpenses = new [] { 1, 2 },
                Expenses = CreateExpanseForms()
            };
            var mapper = CreateMapper();

            // act
            var expenseForms = mapper.Map<List<ExpenseForm>>(expenseLayoutForm);

            // assert
            Assert.Collection(expenseForms,
                item => Assert.Equal(1, item.Order),
                item => Assert.Equal(2, item.Order));
        }
        #endregion

        #region CreateMap<ClassPreferenceExpense, ExpenseForm>() tests
        [Fact]
        public void Map_ClassPreferenceExpense_ReturnsExpenseForm()
        {
            // arrange
            var classPreferenceExpense = new ClassPreferenceExpense
            {
                Order = 1,
                Text = "Text",
                ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                Value = 1,
            };
            var mapper = CreateMapper();

            // act
            var expenseForm = mapper.Map<ExpenseForm>(classPreferenceExpense);

            // assert
            Assert.Equal(1, expenseForm.Order);
            Assert.Equal("Text", expenseForm.Text);
            Assert.Equal(ClassExpenseTypeEnum.Custom, expenseForm.ClassExpenseTypeId);
            Assert.Equal(1, expenseForm.Value);
        }
        #endregion

        #region CreateMap<List<ExpenseForm>, ExpensesLayoutForm>() tests
        [Fact]
        public void Map_ExpenseForms_ReturnsExpenseLayoutForm()
        {
            // arrange
            var expenseForms = CreateExpanseForms();
            var mapper = CreateMapper();

            // act
            var expensesLayoutForm = mapper.Map<ExpensesLayoutForm>(expenseForms);

            // assert
            Assert.Equal(0, expensesLayoutForm.EntityId);
            Assert.Equal(new [] { 1, 2, 3 }, expensesLayoutForm.EnabledExpenses);
            Assert.Equal(FinancialFormConstants.OriginalExpenses.Length, expensesLayoutForm.Expenses.Count);
        }

        [Fact]
        public void Map_ExpenseForms_ReturnsExpenseLayoutFormWithChangedExpense()
        {
            // arrange
            var expenseForms = new List<ExpenseForm>()
            {
                new ExpenseForm
                {
                    Order = 3,
                    Text = "thirdFee",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.FoiRoyaltyFee,
                    Value = 3
                }
            };
            var mapper = CreateMapper();

            // act
            var expenseLayoutForm = mapper.Map<ExpensesLayoutForm>(expenseForms);
            var expense = expenseLayoutForm.Expenses[2];

            // assert
            Assert.Equal(3, expense.Order);
            Assert.Equal("thirdFee", expense.Text);
            Assert.Equal(ClassExpenseTypeEnum.FoiRoyaltyFee, expense.ClassExpenseTypeId);
            Assert.Equal(3, expense.Value);
        }

        [Fact]
        public void Map_EmptyExpenseForms_ReturnsExpenseLayoutFormWithOriginalExpenses()
        {
            // arrange
            var expenseForms = new List<ExpenseForm>();
            var mapper = CreateMapper();

            // act
            var expenseLayoutForm = mapper.Map<ExpensesLayoutForm>(expenseForms);
            var expense = expenseLayoutForm.Expenses[1];

            // assert
            Assert.Equal(2, expense.Order);
            Assert.Equal(FinancialFormConstants.OriginalExpenses[1], expense.Text);
            Assert.Equal(ClassExpenseTypeEnum.Custom, expense.ClassExpenseTypeId);
            Assert.Null(expense.Value);

        }
        #endregion

        #region CreateMap<List<ClassPreferenceExpense>, ExpensesLayoutForm>() tests
        [Fact]
        public void Map_ClassPreferenceExpenses_ReturnsExpensesLayoutForm()
        {
            // arrange
            var classPreferenceExpenses = CreateClassPreferenceExpenses();
            var mapper = CreateMapper();

            // act
            var expensesLayoutForm = mapper.Map<ExpensesLayoutForm>(classPreferenceExpenses);

            // assert
            Assert.Equal(0, expensesLayoutForm.EntityId);
            Assert.Equal(new [] { 1, 2, 3 }, expensesLayoutForm.EnabledExpenses);
            Assert.Equal(FinancialFormConstants.OriginalExpenses.Length, expensesLayoutForm.Expenses.Count);
            Assert.Equal("firstFee", expensesLayoutForm.Expenses[0].Text);
        }
        #endregion

        #region CreateMap<ExpenseForm, ClassExpense>() tests
        [Fact]
        public void Map_ExpenseForm_ReturnsClassExpense()
        {
            // arrange
            var expenseForm = new ExpenseForm
            {
                Order = 1,
                Text = "Text",
                ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                Value = 1
            };
            var mapper = CreateMapper();

            // act
            var classExpense = mapper.Map<ClassExpense>(expenseForm);

            // assert
            Assert.Equal(1, classExpense.Order);
            Assert.Equal("Text", classExpense.Text);
            Assert.Equal(ClassExpenseTypeEnum.Custom, classExpense.ClassExpenseTypeId);
            Assert.Equal(1, classExpense.Value);
        }
        #endregion

        #region CreateMap<ExpensesLayoutForm, List<ClassExpense>>() tests
        [Fact]
        public void Map_ExpensesLayoutForm_ReturnsClassExpenses()
        {
            // arrange
            var form = new ExpensesLayoutForm
            {
                EntityId = 1,
                EnabledExpenses = new[] { 1, 3 },
                Expenses = CreateExpanseForms()
            };
            var mapper = CreateMapper();

            // act
            var classExpenses = mapper.Map<List<ClassExpense>>(form);

            // assert
            Assert.Collection(classExpenses,
                item => Assert.Equal(1, item.Order),
                item => Assert.Equal(3, item.Order));
        }
        #endregion

        #region CreateMap<ClassExpense, ExpenseForm>() tests
        [Fact]
        public void Map_ClassExpense_ReturnsExpenseForm()
        {
            // arrange
            var classExpense = new ClassExpense
            {
                Order = 1,
                Text = "Text",
                ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                Value = 1
            };
            var mapper = CreateMapper();

            // act
            var expenseForm = mapper.Map<ClassExpense>(classExpense);

            // assert
            Assert.Equal(1, expenseForm.Order);
            Assert.Equal("Text", expenseForm.Text);
            Assert.Equal(ClassExpenseTypeEnum.Custom, expenseForm.ClassExpenseTypeId);
            Assert.Equal(1, expenseForm.Value);
        }
        #endregion

        #region CreateMap<List<ClassExpense>, ExpensesLayoutForm>() tests
        [Fact]
        public void Map_ClassExpenses_ReturnsExpensesLayoutForm()
        {
            // arrange
            var classExpenses = CreateClassExpenses();
            var mapper = CreateMapper();

            // act
            var expensesLayoutForm = mapper.Map<ExpensesLayoutForm>(classExpenses);

            // assert
            Assert.Equal(0, expensesLayoutForm.EntityId);
            Assert.Equal(new [] { 1, 2}, expensesLayoutForm.EnabledExpenses);
            Assert.Equal(FinancialFormConstants.OriginalExpenses.Length, expensesLayoutForm.Expenses.Count);
            Assert.Equal("firstFee", expensesLayoutForm.Expenses[0].Text);
        }
        #endregion
        
        #region private methods
        private Mapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ClassExpenseProfile(classDbMock.Object));
            });
            return new Mapper(mapperConfiguration);
        }

        private void AddClassExpenseTypes()
        {
            var expenseTypes = new List<ClassExpenseType>()
            {
                new ClassExpenseType()
                {
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Description = "Custom"
                },
                new ClassExpenseType()
                {
                    ClassExpenseTypeId = ClassExpenseTypeEnum.FoiRoyaltyFee,
                    Description = "FOI Royalty Fee"
                }
            };

            classDbMock.Setup(x => x.GetClassExpenseTypes()).Returns(expenseTypes);
        }

        private List<ClassPreferenceExpense> CreateClassPreferenceExpenses()
        {
            return new List<ClassPreferenceExpense>()
            {
                new ClassPreferenceExpense
                {
                    Order = 1,
                    Text = "firstFee",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 1
                },
                new ClassPreferenceExpense
                {
                    Order = 2,
                    Text = "secondFee",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 2
                },
                new ClassPreferenceExpense
                {
                    Order = 3,
                    Text = "thirdFee",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 3
                }
            };
        }

        private List<ExpenseForm> CreateExpanseForms()
        {
            return new List<ExpenseForm>()
            {
                new ExpenseForm
                {
                    Order = 1,
                    Text = "firstFee",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 1
                },
                new ExpenseForm
                {
                    Order = 2,
                    Text = "secondFee",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 2
                },
                new ExpenseForm
                {
                    Order = 3,
                    Text = "thirdFee",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 3
                }
            };
        }

        private List<ClassExpense> CreateClassExpenses()
        {
            return new List<ClassExpense>() {
                new ClassExpense
                {
                    Order = 1,
                    Text = "firstFee",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 1
                },
                new ClassExpense
                {
                    Order = 2,
                    Text = "secondFee",
                    ClassExpenseTypeId = ClassExpenseTypeEnum.Custom,
                    Value = 2
                }
            };
        }
        #endregion
    }
}
