using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using AutomationSystem.Main.Core.DistanceClassTemplates.System.JobRunExecutors;
using AutomationSystem.Main.Core.DistanceClassTemplates.System.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Model;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using Xunit;

namespace AutomationSystem.Main.Tests.DistanceClassTemplates.System.JobRunExecutors
{
    public class DistanceCompletionJobExecutorTests
    {
        private const long JobRunId = 30;
        private const long ClassId = 300;
        private const long DistanceTemplateId = 1;

        private readonly Mock<IDistanceClassTemplateService> distanceTemplateServiceMock;
        private readonly Mock<IDistanceClassTemplateDatabaseLayer> distanceTemplateDbMock;
        private readonly Mock<ICoreEmailService> coreEmailServiceMock;
        private readonly Mock<ITracerFactory> tracerFactoryMock;

        private readonly JobRun jobRun;
        private readonly DistanceClassTemplateCompletionResult completionResult;

        public DistanceCompletionJobExecutorTests()
        {
            distanceTemplateServiceMock = new Mock<IDistanceClassTemplateService>();
            distanceTemplateDbMock = new Mock<IDistanceClassTemplateDatabaseLayer>();
            coreEmailServiceMock = new Mock<ICoreEmailService>();
            tracerFactoryMock = new Mock<ITracerFactory>();
            tracerFactoryMock.Setup(e => e.CreateTracer<DistanceCompletionJobExecutor>(It.IsAny<object>()))
                .Returns(Mock.Of<ITracer>());

            jobRun = new JobRun
            {
                JobRunId = JobRunId
            };

            var distanceTemplate = new DistanceClassTemplate
            {
                DistanceClassTemplateId = DistanceTemplateId,
            };
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplatesByFilter(It.IsAny<DistanceClassTemplateFilter>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new List<DistanceClassTemplate> { distanceTemplate });

            completionResult = new DistanceClassTemplateCompletionResult(DistanceTemplateId)
            {
                IsSuccess = true
            };
            distanceTemplateServiceMock.Setup(e => e.CompleteDistanceClassTemplate(It.IsAny<long>(), It.IsAny<string>())).Returns(completionResult);
        }

        #region CanReportIncident

        [Fact]
        public void CanReportIncident_IsTrue()
        {
            // arrange
            var job = CreateJob();

            // act & assert
            Assert.True(job.CanReportIncident);
        }

        #endregion

        #region Execute() tests

        [Fact]
        public void Execute_ExpectedFilterIsPassedWhenDistanceTemplatesLoading()
        {
            // arrange
            var filter = (DistanceClassTemplateFilter) null;
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplatesByFilter(It.IsAny<DistanceClassTemplateFilter>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Callback((DistanceClassTemplateFilter f, DistanceClassTemplateIncludes i) => filter = f)
                .Returns(new List<DistanceClassTemplate>());

            var now = DateTime.UtcNow;
            var expectedDateFrom = now.AddMinutes(-5);
            var expectedDateTo = now.AddMinutes(5);

            var job = CreateJob();

            // act
            job.Execute(jobRun);

            // assert
            Assert.NotNull(filter);
            Assert.Equal(DistanceClassTemplateState.Approved, filter.TemplateState);
            Assert.True(expectedDateFrom <= filter.ToAutomationCompleteTime && filter.ToAutomationCompleteTime < expectedDateTo);
        }

        [Fact]
        public void Execute_NoTemplateToProcess_ReturnsSuccessDoNotSendReport()
        {
            // arrange
            distanceTemplateDbMock.Setup(e => e.GetDistanceClassTemplatesByFilter(It.IsAny<DistanceClassTemplateFilter>(), It.IsAny<DistanceClassTemplateIncludes>()))
                .Returns(new List<DistanceClassTemplate>());

            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.True(result.IsSuccess);
            VerifyReportSend(false);
        }

        [Fact]
        public void Execute_TemplateProcessingSuccess_ReturnsSuccessSendReport()
        {
            // arrange
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.True(result.IsSuccess);
            VerifyReportSend(true);
            distanceTemplateServiceMock.Verify(e => e.CompleteDistanceClassTemplate(DistanceTemplateId, It.IsAny<string>()));
        }

        [Fact]
        public void Execute_TemplateProcessingFailsOnClass_ReturnsFailAndSentReport()
        {
            // arrange
            completionResult.IsSuccess = false;
            completionResult.CorruptedClassId = ClassId;
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.False(result.IsSuccess);

            Assert.NotNull(result.Incident);
            AssertIncident(result.Incident, IncidentTypeEnum.JobRunError, EntityTypeEnum.CoreJobRun, JobRunId);

            var childIncident = result.Incident.Children.FirstOrDefault();
            Assert.NotNull(childIncident);
            AssertIncident(childIncident, IncidentTypeEnum.JobRunError, EntityTypeEnum.MainClass, ClassId);

            VerifyReportSend(true);
        }

        [Fact]
        public void Execute_TemplateProcessingFailsNoClassSpecified_ReturnsFailAndSentReport()
        {
            // arrange
            completionResult.IsSuccess = false;
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.False(result.IsSuccess);

            Assert.NotNull(result.Incident);
            AssertIncident(result.Incident, IncidentTypeEnum.JobRunError, EntityTypeEnum.CoreJobRun, JobRunId);

            var childIncident = result.Incident.Children.FirstOrDefault();
            Assert.NotNull(childIncident);
            AssertIncident(childIncident, IncidentTypeEnum.JobRunError, EntityTypeEnum.MainDistanceClassTemplate, DistanceTemplateId);

            VerifyReportSend(true);
        }

        [Fact]
        public void Execute_CompletionCausesException_ReturnsFailAndDoNotSendReport()
        {
            // arrange
            distanceTemplateServiceMock.Setup(e => e.CompleteDistanceClassTemplate(It.IsAny<long>(), It.IsAny<string>())).Throws(new Exception());
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.False(result.IsSuccess);

            Assert.NotNull(result.Incident);
            AssertIncident(result.Incident, IncidentTypeEnum.JobRunError, EntityTypeEnum.CoreJobRun, JobRunId);

            var childIncident = result.Incident.Children.FirstOrDefault();
            Assert.NotNull(childIncident);
            AssertIncident(childIncident, IncidentTypeEnum.JobRunError, EntityTypeEnum.MainDistanceClassTemplate, DistanceTemplateId);

            VerifyReportSend(false);
        }

        #endregion

        #region private methods

        private void VerifyReportSend(bool expectedSending)
        {
            if (expectedSending)
            {
                coreEmailServiceMock.Verify(
                    e => e.SendJobReportEmail(
                        EmailTypeEnum.DistanceClassCompletionJobReport,
                        (int)SeverityEnum.High,
                        JobRunId,
                        It.Is<Dictionary<string, object>>(x => x.ContainsKey("DistanceClassCompletionJobReport"))),
                    Times.Once);
            }
            else
            {
                coreEmailServiceMock.Verify(
                    e => e.SendJobReportEmail(It.IsAny<EmailTypeEnum>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<Dictionary<string, object>>()),
                    Times.Never);
            }
        }

        private void AssertIncident(IncidentForLog incident, IncidentTypeEnum incidentType, EntityTypeEnum? entityType, long? entityId)
        {
            Assert.Equal(incidentType, incident.IncidentTypeId);
            Assert.Equal(entityType, incident.EntityTypeId);
            Assert.Equal(entityId, incident.EntityId);
        }

        private DistanceCompletionJobExecutor CreateJob()
        {
            return new DistanceCompletionJobExecutor(
                distanceTemplateServiceMock.Object,
                distanceTemplateDbMock.Object,
                coreEmailServiceMock.Object,
                tracerFactoryMock.Object);
        }

        #endregion
    }
}
