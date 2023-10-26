using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Home.System.Incidents;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using Xunit;

namespace AutomationSystem.Main.Tests.Home.System.Incidents
{
    public class MissingClassIdInUrlIncidentHandlerTests
    {
        #region CanHandle() tests

        [Theory]
        [InlineData("The parameters dictionary contains a null entry for parameter 'classId'", true)]
        [InlineData("The parameters dictionary contains a null entry for parameter 'classId' another arbitrary text", true)]
        [InlineData("The parameters dictionary contains a null entry for parameter", false)]
        [InlineData("Something completely different", false)]
        public void CanHandle_ForSpecifiedIncidentMessage_ExpectedResultIsReturned(string message, bool expectedResult)
        {
            // arrange
            var incidentForLog = IncidentForLog.New(IncidentTypeEnum.EmailError, message);
            var handler = CreateHandler();

            // act
            var result = handler.CanHandle(incidentForLog);

            // assert
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region HandleIncident() tests

        [Fact]
        public void HandleIncident_ForAnyIncident_HandlerReturnsProcessAsHidden()
        {
            // arrange
            var incidentForLog = IncidentForLog.New(IncidentTypeEnum.EmailError, "message");
            var handler = CreateHandler();

            // act
            var result = handler.HandleIncident(incidentForLog);

            // assert
            Assert.Equal(IncidentOperationType.ProcessAsHidden, result);
        }

        #endregion

        #region private methods

        private MissingClassIdInUrlIncidentHandler CreateHandler()
        {
            return new MissingClassIdInUrlIncidentHandler();
        }

        #endregion
    }
}
