using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceProfiles.AppLogic.EventCheckers
{
    public class ProfileToDeleteHasNoDistanceProfileEventCheckerTests
    {
        private readonly Mock<IDistanceProfileDatabaseLayer> distanceProfileDbMock;

        public ProfileToDeleteHasNoDistanceProfileEventCheckerTests()
        {
            distanceProfileDbMock = new Mock<IDistanceProfileDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_ProfileDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.AnyDistanceProfileOnProfile(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new ProfileDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            distanceProfileDbMock.Verify(e => e.AnyDistanceProfileOnProfile(1), Times.Once);
        }
        #endregion

        #region private methods
        private ProfileToDeleteHasNoDistanceProfileEventChecker CreateChecker()
        {
            return new ProfileToDeleteHasNoDistanceProfileEventChecker(distanceProfileDbMock.Object);
        }
        #endregion
    }
}