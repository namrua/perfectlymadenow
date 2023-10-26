using AutomationSystem.Main.Core.Classes.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.EventCheckers
{
    public class PriceListToDeleteHasNoClassEventCheckerTests
    {
        private readonly Mock<IClassDatabaseLayer> classDbMock;

        public PriceListToDeleteHasNoClassEventCheckerTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_PriceListDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            classDbMock.Setup(e => e.PriceListOnAnyClass(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new PriceListDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            classDbMock.Verify(e => e.PriceListOnAnyClass(1), Times.Once);
        }
        #endregion

        #region private methods
        private PriceListToDeleteHasNoClassEventChecker CreateChecker()
        {
            return new PriceListToDeleteHasNoClassEventChecker(classDbMock.Object);
        }
        #endregion
    }
}
