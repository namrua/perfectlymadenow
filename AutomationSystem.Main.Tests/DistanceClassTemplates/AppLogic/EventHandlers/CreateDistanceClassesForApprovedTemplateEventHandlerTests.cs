using Xunit;
using Moq;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventHandlers;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic.EventHandlers
{
    public class CreateDistanceClassesForApprovedTemplateEventHandlerTests
    {
        private readonly Mock<IDistanceClassTemplateService> serviceMock;

        public CreateDistanceClassesForApprovedTemplateEventHandlerTests()
        {
            serviceMock = new Mock<IDistanceClassTemplateService>();
        }

        #region HandleEvent() tests
        [Fact]
        public void HandleEvent_DistanceClassTemplateApprovedEvent_ReturnsResultSuccess()
        {
            // arrange
            var approvedEvent = new DistanceClassTemplateApprovedEvent(1);
            serviceMock.Setup(e => e.PopulateDistanceClassesForDistanceTemplate(It.IsAny<long>()));
            var handler = CreateEventHandler();

            // act
            var result = handler.HandleEvent(approvedEvent);

            // assert
            Assert.Equal(ResultType.Success, result.ResultType);
            serviceMock.Verify(e => e.PopulateDistanceClassesForDistanceTemplate(1), Times.Once);
        }
        #endregion

        #region private methods
        private CreateDistanceClassesForApprovedTemplateEventHandler CreateEventHandler()
        {
            return new CreateDistanceClassesForApprovedTemplateEventHandler(serviceMock.Object);
        }
        #endregion
    }
}
