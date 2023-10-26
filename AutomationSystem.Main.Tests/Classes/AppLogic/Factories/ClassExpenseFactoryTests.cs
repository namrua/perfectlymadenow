using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.AppLogic.Factories;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Model;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.Factories
{
    public class ClassExpenseFactoryTests
    {
        private readonly Mock<IClassDatabaseLayer> classDbMock;

        public ClassExpenseFactoryTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
            AddClassExpenseTypes();
        }

        #region CreateExpensesLayoutForEdit() tests
        [Fact]
        public void CreateExpensesLayoutForEdit_ReturnsExpensesLayoutForEdit()
        {
            // arrange
            var factory = CreateClassExpenseFactory();
            

            // act
            var expenseLayoutForEdit = factory.CreateExpensesLayoutForEdit(new Currency());

            // assert
            Assert.Collection(expenseLayoutForEdit.ExpenseTypes,
                item =>
                {
                    Assert.Equal("Custom", item.Description);
                    Assert.Equal(ClassExpenseTypeEnum.Custom, item.ClassExpenseTypeId);
                },
                item =>
                {
                    Assert.Equal("FOI Royalty Fee", item.Description);
                    Assert.Equal(ClassExpenseTypeEnum.FoiRoyaltyFee, item.ClassExpenseTypeId);
                });
        }

        [Fact]
        public void CreateExpenseLayoutForEdit_CurrencyIsPassedToCurrencyCode()
        {
            // arrange
            var currency = new Currency
            {
                Name = "Name"
            };
            var factory = CreateClassExpenseFactory();

            // act
            var forEdit = factory.CreateExpensesLayoutForEdit(currency);
            // assert
            Assert.Equal("Name", forEdit.CurrencyCode);
        }
        #endregion

        #region private methods
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

        public ClassExpenseFactory CreateClassExpenseFactory()
        {
            return new ClassExpenseFactory(classDbMock.Object);
        }
        #endregion
    }
}
