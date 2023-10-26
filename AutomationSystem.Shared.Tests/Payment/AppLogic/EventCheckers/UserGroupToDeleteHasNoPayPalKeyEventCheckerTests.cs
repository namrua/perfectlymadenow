using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities.Models.Events;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Core.Payment.AppLogic.EventCheckers;
using Moq;
using Xunit;

namespace AutomationSystem.Shared.Tests.Payment.AppLogic.EventCheckers
{
    public class UserGroupToDeleteHasNoPayPalKeyEventCheckerTests
    {
        private readonly Mock<IPaymentDatabaseLayer> paymentDbMock;

        public UserGroupToDeleteHasNoPayPalKeyEventCheckerTests()
        {
            paymentDbMock = new Mock<IPaymentDatabaseLayer>();
        }

        #region CheckEvent() tests
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CheckEvent_UserGroupDeletingEvent_ReturnsExpectedResult(bool eventValue, bool expectedResult)
        {
            // arrange
            paymentDbMock.Setup(e => e.AnyPayPalKeyOnUserGroup(It.IsAny<long>(), It.IsAny<UserGroupTypeEnum>())).Returns(eventValue);
            var checker = CreateChecker();

            // act
            var result = checker.CheckEvent(new UserGroupDeletingEvent(1, UserGroupTypeEnum.MainProfile));

            // assert
            Assert.Equal(expectedResult, result);
            paymentDbMock.Verify(e => e.AnyPayPalKeyOnUserGroup(1, UserGroupTypeEnum.MainProfile), Times.Once);
        }
        #endregion

        #region private methods
        private UserGroupToDeleteHasNoPayPalKeyEventChecker CreateChecker()
        {
            return new UserGroupToDeleteHasNoPayPalKeyEventChecker(paymentDbMock.Object);
        }
        #endregion
    }
}
