using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models.Events;
using AutomationSystem.Shared.Core.ConferenceAccounts.AppLogic.EventCheckers;
using AutomationSystem.Shared.Core.ConferenceAccounts.Data;
using Moq;
using Xunit;

namespace AutomationSystem.Shared.Tests.ConferenceAccounts.AppLogic.EventCheckers
{
    public class UserGroupToDeleteHasNoConferenceAccountEventCheckerTests
    {
        private readonly Mock<IConferenceAccountDatabaseLayer> conferenceDbMock;

        public UserGroupToDeleteHasNoConferenceAccountEventCheckerTests()
        {
            conferenceDbMock = new Mock<IConferenceAccountDatabaseLayer>();
        }

        #region CheckEvent()tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_UserGroupDeletingEvent_ReturnsExpectedResul(bool eventValue, bool expectedResult)
        {
            // arrange
            conferenceDbMock.Setup(e => e.AnyConferenceAccountOnUserGroup(It.IsAny<long>(), It.IsAny<UserGroupTypeEnum>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new UserGroupDeletingEvent(1, UserGroupTypeEnum.MainProfile));

            // assert
            Assert.Equal(expectedResult, result);
            conferenceDbMock.Verify(e => e.AnyConferenceAccountOnUserGroup(1, UserGroupTypeEnum.MainProfile), Times.Once);
        }
        #endregion

        #region private methods
        private UserGroupToDeleteHasNoConferenceAccountEventChecker CreateChecker()
        {
            return new UserGroupToDeleteHasNoConferenceAccountEventChecker(conferenceDbMock.Object);
        }
        #endregion
    }
}
