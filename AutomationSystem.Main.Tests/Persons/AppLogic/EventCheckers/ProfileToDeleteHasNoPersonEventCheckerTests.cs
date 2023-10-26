using AutomationSystem.Main.Core.Persons.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Persons.AppLogic.EventCheckers
{
    public class ProfileToDeleteHasNoPersonEventCheckerTests
    {
        private readonly Mock<IPersonDatabaseLayer> personDbMock;

        public ProfileToDeleteHasNoPersonEventCheckerTests()
        {
            personDbMock = new Mock<IPersonDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_ProfileDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            personDbMock.Setup(e => e.AnyPersonOnProfile(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new ProfileDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            personDbMock.Verify(e => e.AnyPersonOnProfile(1), Times.Once);
        }
        #endregion

        #region private methods
        private ProfileToDeleteHasNoPersonEventChecker CreateChecker()
        {
            return new ProfileToDeleteHasNoPersonEventChecker(personDbMock.Object);
        }
        #endregion
    }
}
