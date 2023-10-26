using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Preferences.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.Preferences.System;
using Moq;
using Xunit;

namespace AutomationSystem.Main.Tests.Preferences.AppLogic.EventCheckers
{
    public class PersonToDeleteNotUsedInPreferencesTests
    {
        private readonly Mock<IMainPreferenceProvider> preferenceProviderMock;

        public PersonToDeleteNotUsedInPreferencesTests()
        {
            preferenceProviderMock = new Mock<IMainPreferenceProvider>();
        }

        #region EventChecker() tests
        [Theory]
        [InlineData(1, false)]
        [InlineData(20, true)]
        public void CheckEvent_PersonDeletingEvent_ReturnsExpectedResult(long eventValue, bool expectedResult)
        {
            // arrange
            preferenceProviderMock.Setup(e => e.GetMasterLeadInstructorId()).Returns(1);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new PersonDeletingEvent(eventValue));

            // assert
            Assert.Equal(expectedResult, result);
            preferenceProviderMock.Verify(e => e.GetMasterLeadInstructorId(), Times.Once);
        }
        #endregion

        #region private methods
        private PersonToDeleteNotUsedInPreferences CreateChecker()
        {
            return new PersonToDeleteNotUsedInPreferences(preferenceProviderMock.Object);
        }
        #endregion
    }
}
