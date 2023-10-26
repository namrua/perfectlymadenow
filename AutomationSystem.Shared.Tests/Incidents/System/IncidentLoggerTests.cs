using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Core.Incidents.Data;
using AutomationSystem.Shared.Core.Incidents.System;
using AutomationSystem.Shared.Model;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using Xunit;

namespace AutomationSystem.Shared.Tests.Incidents.System
{
    public class IncidentLoggerTests
    {
        private readonly Mock<ITracerFactory> tracerFactoryMock;
        private readonly Mock<IIncidentDatabaseLayer> incidentDbMock;
        private readonly Mock<ICoreAsyncRequestManager> coreAsyncRequestManagerMock;
        private readonly Mock<ICoreMapper> coreMapperMock;
        private readonly Mock<IIncidentHandler> incidentHandlerMock;

        private readonly Incident incidentToInsert;
        private readonly IncidentForLog incidentForLog;

        public IncidentLoggerTests()
        {
            tracerFactoryMock = new Mock<ITracerFactory>();
            incidentDbMock = new Mock<IIncidentDatabaseLayer>();
            coreAsyncRequestManagerMock = new Mock<ICoreAsyncRequestManager>();
            coreMapperMock = new Mock<ICoreMapper>();
            incidentHandlerMock = new Mock<IIncidentHandler>();

            incidentToInsert = new Incident();
            incidentForLog = IncidentForLog.New(IncidentTypeEnum.EmailError, "Test message");

            tracerFactoryMock.Setup(e => e.CreateTracer<IncidentLogger>(It.IsAny<object>())).Returns(Mock.Of<ITracer>());
        }

        #region LogIncident() tests

        [Fact]
        public void LogIncident_NullIncidentForLog_ThrowsArgumentNullException()
        {
            // arrange
            SetupLogIncident();
            var logger = CreateLogger();

            // act & assert
            Assert.Throws<ArgumentNullException>(() => logger.LogIncident(null));
        }

        [Fact]
        public void LogIncident_CanReportIncident_CreatesIncidentAndAddsReportRequest()
        {
            // arrange
            SetupLogIncident();
            var logger = CreateLogger();

            // act
            var result = logger.LogIncident(incidentForLog);

            // assert
            Assert.Equal(1, result);
            incidentHandlerMock.Verify(e => e.HandleIncident(It.IsAny<IncidentForLog>()), Times.Never);
            incidentDbMock.Verify(e => e.InsertIncident(incidentToInsert), Times.Once);
            coreAsyncRequestManagerMock.Verify(e => e.AddReportIncidentRequest(1, (int) SeverityEnum.High), Times.Once);
        }


        [Fact]
        public void LogIncident_CannotReportIncident_CreatesIncidentWithoutAddingReportingRequest()
        {
            // arrange
            SetupLogIncident(canBeReported: false);
            var logger = CreateLogger();

            // act
            var result = logger.LogIncident(incidentForLog);

            // assert
            Assert.Equal(1, result);
            incidentHandlerMock.Verify(e => e.HandleIncident(It.IsAny<IncidentForLog>()), Times.Never);
            incidentDbMock.Verify(e => e.InsertIncident(incidentToInsert), Times.Once);
            coreAsyncRequestManagerMock.Verify(e => e.AddReportIncidentRequest(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void LogIncident_IncidentHandlerReturnsProcess_CreatesIncidentAndAddsReportRequest()
        {
            // arrange
            SetupLogIncident(canHandle: true, operationType: IncidentOperationType.Process);
            var logger = CreateLogger();

            // act
            var result = logger.LogIncident(incidentForLog);

            // assert
            Assert.Equal(1, result);
            Assert.False(incidentToInsert.IsHidden);
            incidentHandlerMock.Verify(e => e.HandleIncident(incidentForLog), Times.Once);
            incidentDbMock.Verify(e => e.InsertIncident(incidentToInsert), Times.Once);
            coreAsyncRequestManagerMock.Verify(e => e.AddReportIncidentRequest(1, (int)SeverityEnum.High), Times.Once);
        }

        [Fact]
        public void LogIncident_IncidentHandlerReturnsProcessAsHidden_CreatesIncidentAsHiddenWithoutAddingReportingRequest()
        {
            // arrange
            SetupLogIncident(canHandle: true, operationType: IncidentOperationType.ProcessAsHidden);
            var logger = CreateLogger();

            // act
            var result = logger.LogIncident(incidentForLog);

            // assert
            Assert.Equal(1, result);
            Assert.True(incidentToInsert.IsHidden);
            Assert.All(incidentToInsert.IncidentChildren, item => Assert.True(item.IsHidden));
            incidentHandlerMock.Verify(e => e.HandleIncident(incidentForLog), Times.Once);
            incidentDbMock.Verify(e => e.InsertIncident(incidentToInsert), Times.Once);
            coreAsyncRequestManagerMock.Verify(e => e.AddReportIncidentRequest(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void LogIncident_IncidentHandlerReturnsIgnore_IncidentIsIgnored()
        {
            // arrange
            SetupLogIncident(canHandle: true, operationType: IncidentOperationType.Ignore);
            var logger = CreateLogger();

            // act
            var result = logger.LogIncident(incidentForLog);

            // assert
            Assert.Null(result);
            incidentHandlerMock.Verify(e => e.HandleIncident(incidentForLog), Times.Once);
            incidentDbMock.Verify(e => e.InsertIncident(It.IsAny<Incident>()), Times.Never);
            coreAsyncRequestManagerMock.Verify(e => e.AddReportIncidentRequest(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
        }

        #endregion

        #region private methods

        private void SetupLogIncident(bool canHandle = false, IncidentOperationType operationType = IncidentOperationType.Process, bool canBeReported = true)
        {
            incidentToInsert.CanBeReport = canBeReported;
            incidentToInsert.IncidentChildren.Add(new Incident());
            coreMapperMock.Setup(e => e.Map<Incident>(It.IsAny<object>())).Returns(incidentToInsert);

            incidentDbMock.Setup(e => e.InsertIncident(It.IsAny<Incident>())).Returns(1);

            incidentHandlerMock.Setup(e => e.HandlerCode).Returns("TestHandler");
            incidentHandlerMock.Setup(e => e.CanHandle(It.IsAny<IncidentForLog>())).Returns(canHandle);
            incidentHandlerMock.Setup(e => e.HandleIncident(It.IsAny<IncidentForLog>())).Returns(operationType);
        }

        private IncidentLogger CreateLogger()
        {
            return new IncidentLogger(
                incidentDbMock.Object,
                coreAsyncRequestManagerMock.Object,
                coreMapperMock.Object,
                tracerFactoryMock.Object,
                new [] { incidentHandlerMock.Object });
        }

        #endregion
    }
}
