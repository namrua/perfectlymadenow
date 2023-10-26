using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceProfiles.AppLogic.EventCheckers
{
    public class PayPalAccountToDeleteHasNoDistanceProfileEventCheckerTests
    {
        private readonly Mock<IDistanceProfileDatabaseLayer> distanceProfileDbMock;

        public PayPalAccountToDeleteHasNoDistanceProfileEventCheckerTests()
        {
            distanceProfileDbMock = new Mock<IDistanceProfileDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_PayPalAccountDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.PayPalKeyOnAnyDistanceProfile(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new PayPalAccountDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            distanceProfileDbMock.Verify(e => e.PayPalKeyOnAnyDistanceProfile(1), Times.Once);
        }
        #endregion

        #region private methods
        private PayPalAccountToDeleteHasNoDistanceProfileEventChecker CreateChecker()
        {
            return new PayPalAccountToDeleteHasNoDistanceProfileEventChecker(distanceProfileDbMock.Object);
        }
        #endregion
    }
}
