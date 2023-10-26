using Xunit;
using Moq;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.EventHandlers;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.AppLogic.EventHandlers
{
    public class ChangesPropagatedToDistanceClassesEventHandlerTests
    {
        private readonly Mock<IDistanceClassTemplateService> serviceMock;
        private readonly Mock<IDistanceClassTemplateDatabaseLayer> templateDbMock;
        private readonly Mock<IDistanceClassTemplateHelper> helperMock;

        public ChangesPropagatedToDistanceClassesEventHandlerTests()
        {
            serviceMock = new Mock<IDistanceClassTemplateService>();
            templateDbMock = new Mock<IDistanceClassTemplateDatabaseLayer>();
            helperMock = new Mock<IDistanceClassTemplateHelper>();
        }

        #region HandleEvent() tests
        [Fact]
        public void HandleEvent_IncludesPassToGetDistanceClassTemplateById_ReturnsDistanceClassTemplateFromDb()
        {
            // arrange
            templateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(new DistanceClassTemplate());
            var changedEvent = new DistanceClassTemplateChangedEvent(1);
            var handler = CreateEventHandler();

            // act
            handler.HandleEvent(changedEvent);

            // assert
            templateDbMock.Verify(e => e.GetDistanceClassTemplateById(1, DistanceClassTemplateIncludes.DistanceClassTemplatePersons), Times.Once);
        }

        [Theory]
        [InlineData(DistanceClassTemplateState.Completed)]
        [InlineData(DistanceClassTemplateState.New)]
        public void HandleEvent_DistanceClassTemplateIsNotApproved_ReturnsResultSkipped(DistanceClassTemplateState state)
        {
            // arrange
            var changedEvent = new DistanceClassTemplateChangedEvent(1);
            var template = new DistanceClassTemplate();
            templateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(template);
            helperMock.Setup(e => e.GetDistanceClassTemplateState(It.IsAny<DistanceClassTemplate>())).Returns(state);
            var handler = CreateEventHandler();

            // act
            var result = handler.HandleEvent(changedEvent);

            // assert
            Assert.Equal(ResultType.Skipped, result.ResultType);
            helperMock.Verify(e => e.GetDistanceClassTemplateState(template), Times.Once);
            serviceMock.Verify(e => e.PropagateChangesToDistanceClasses(It.IsAny<DistanceClassTemplate>()), Times.Never);
        }

        [Fact]
        public void HandleEvent_DistanceClassTemplateIsApproved_CallsPropagateChangesAndReturnsSuccess()
        {
            // arrange
            var changedEvent = new DistanceClassTemplateChangedEvent(2);
            var template = new DistanceClassTemplate();
            var form = new DistanceClassTemplateForm();
            templateDbMock.Setup(e => e.GetDistanceClassTemplateById(It.IsAny<long>(), It.IsAny<DistanceClassTemplateIncludes>())).Returns(template);
            helperMock.Setup(e => e.GetDistanceClassTemplateState(It.IsAny<DistanceClassTemplate>())).Returns(DistanceClassTemplateState.Approved);
            var handler = CreateEventHandler();

            // act
            var result = handler.HandleEvent(changedEvent);

            // assert
            Assert.Equal(ResultType.Success, result.ResultType);
            serviceMock.Verify(e => e.PropagateChangesToDistanceClasses(template), Times.Once);

        }
        #endregion

        #region private methods
        private ChangesPropagatedToDistanceClassesEventHandler CreateEventHandler()
        {
            return new ChangesPropagatedToDistanceClassesEventHandler(
                serviceMock.Object,
                templateDbMock.Object,
                helperMock.Object);
        }
        #endregion
    }
}
