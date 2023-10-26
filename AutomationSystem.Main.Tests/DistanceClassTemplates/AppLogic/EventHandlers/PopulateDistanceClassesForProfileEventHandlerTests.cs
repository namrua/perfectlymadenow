using Xunit;
using Moq;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventHandlers;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic.EventHandlers
{
    public class PopulateDistanceClassesForProfileEventHandlerTests
    {
        private readonly Mock<IDistanceClassTemplateService> serviceMock;

        public PopulateDistanceClassesForProfileEventHandlerTests()
        {
            serviceMock = new Mock<IDistanceClassTemplateService>();
        }

        #region HandleEvent() tests
        [Fact]
        public void HandleEvent_DistanceProfileStatusChangedEventIsActive_ReturnsResultSuccess()
        {
            // arrange
            var changedEvent = new DistanceProfileStatusChangedEvent(1, true);
            serviceMock.Setup(e => e.PopulateDistanceClassesForDistanceProfile(It.IsAny<long>()));
            var handler = CreateHandler();

            // act
            var result = handler.HandleEvent(changedEvent);

            // assert
            Assert.Equal(ResultType.Success, result.ResultType);
            serviceMock.Verify(e => e.PopulateDistanceClassesForDistanceProfile(1), Times.Once);
        }

        [Fact]
        public void HandleEvent_DistanceProfileStatusChangedEventIsDeactive_ReturnsResultSkipped()
        {
            // arrange
            var changedEvent = new DistanceProfileStatusChangedEvent(2, false);
            var handler = CreateHandler();

            // act
            var result = handler.HandleEvent(changedEvent);

            // assert
            Assert.Equal(ResultType.Skipped, result.ResultType);
            serviceMock.Verify(e => e.PopulateDistanceClassesForDistanceProfile(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #region private methods
        private PopulateDistanceClassesForProfileEventHandler CreateHandler()
        {
            return new PopulateDistanceClassesForProfileEventHandler(serviceMock.Object);
        }
        #endregion
    }
}
