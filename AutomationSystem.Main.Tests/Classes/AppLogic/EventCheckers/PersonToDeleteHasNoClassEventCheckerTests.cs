using AutomationSystem.Main.Core.Classes.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Classes.AppLogic.EventCheckers
{
    public class PersonToDeleteHasNoClassEventCheckerTests
    {
        private readonly Mock<IClassDatabaseLayer> classDbMock;

        public PersonToDeleteHasNoClassEventCheckerTests()
        {
            classDbMock = new Mock<IClassDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_PersonDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            classDbMock.Setup(e => e.PersonOnAnyClass(It.IsAny<long>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new PersonDeletingEvent(1));

            // assert
            Assert.Equal(expectedResult, result);
            classDbMock.Verify(e => e.PersonOnAnyClass(1), Times.Once);
        }
        #endregion

        #region private methods
        private PersonToDeleteHasNoClassEventChecker CreateChecker()
        {
            return new PersonToDeleteHasNoClassEventChecker(classDbMock.Object);
        }
        #endregion
    }
}
