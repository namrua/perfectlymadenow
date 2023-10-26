using AutomationSystem.Main.Core.Classes.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.EventCheckers
{
    public class PayPalAccountToDeleteHasNoClassEventCheckerTests
    {
        private readonly Mock<IClassDatabaseLayer> classDbMock;

        public PayPalAccountToDeleteHasNoClassEventCheckerTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_PayPalAccountDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            classDbMock.Setup(e => e.PayPalKeyOnAnyClass(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new PayPalAccountDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            classDbMock.Verify(e => e.PayPalKeyOnAnyClass(1), Times.Once);
        }
        #endregion

        #region private methods
        private PayPalAccountToDeleteHasNoClassEventChecker CreateChecker()
        {
            return new PayPalAccountToDeleteHasNoClassEventChecker(classDbMock.Object);
        }
        #endregion
    }
}
