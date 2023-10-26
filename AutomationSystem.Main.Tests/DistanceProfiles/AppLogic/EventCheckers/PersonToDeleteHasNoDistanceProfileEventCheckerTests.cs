using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceProfiles.AppLogic.EventCheckers
{
    public class PersonToDeleteHasNoDistanceProfileEventCheckerTests
    {
        private readonly Mock<IDistanceProfileDatabaseLayer> distanceProfileDbMock;

        public PersonToDeleteHasNoDistanceProfileEventCheckerTests()
        {
            distanceProfileDbMock = new Mock<IDistanceProfileDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_PersonDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            distanceProfileDbMock.Setup(e => e.PersonOnAnyDistanceProfile(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new PersonDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            distanceProfileDbMock.Verify(e => e.PersonOnAnyDistanceProfile(1), Times.Once);
        }
        #endregion

        #region private methods
        private PersonToDeleteHasNoDistanceProfileEventChecker CreateChecker()
        {
            return new PersonToDeleteHasNoDistanceProfileEventChecker(distanceProfileDbMock.Object);
        }
        #endregion
    }
}
