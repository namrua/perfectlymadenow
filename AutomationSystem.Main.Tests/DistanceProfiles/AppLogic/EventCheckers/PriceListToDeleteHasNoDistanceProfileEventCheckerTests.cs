using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceProfiles.AppLogic.EventCheckers
{
    public class PriceListToDeleteHasNoDistanceProfileEventCheckerTests
    {
        private readonly Mock<IDistanceProfileDatabaseLayer> distanceProfileDbMock;

        public PriceListToDeleteHasNoDistanceProfileEventCheckerTests()
        {
            distanceProfileDbMock = new Mock<IDistanceProfileDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_PriceListDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.PriceListOnAnyDistanceProfile(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new PriceListDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            distanceProfileDbMock.Verify(e => e.PriceListOnAnyDistanceProfile(1), Times.Once);
        }
        #endregion

        #region private methods
        private PriceListToDeleteHasNoDistanceProfileEventChecker CreateChecker()
        {
            return new PriceListToDeleteHasNoDistanceProfileEventChecker(distanceProfileDbMock.Object);
        }
        #endregion
    }
}