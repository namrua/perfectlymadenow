using AutomationSystem.Main.Core.Classes.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.EventCheckers
{
    public class ProfileToDeleteHasNoClassEventCheckerTests
    {
        private readonly Mock<IClassDatabaseLayer> classDbMock;

        public ProfileToDeleteHasNoClassEventCheckerTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_ProfileDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            classDbMock.Setup(e => e.AnyClassOnProfile(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new ProfileDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            classDbMock.Verify(e => e.AnyClassOnProfile(1), Times.Once);
        }
        #endregion

        #region private methods
        private ProfileToDeleteHasNoClassEventChecker CreateChecker()
        {
            return new ProfileToDeleteHasNoClassEventChecker(classDbMock.Object);
        }
        #endregion
    }
}
