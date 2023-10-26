using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Maintenance.Data;
using AutomationSystem.Main.Core.Maintenance.System.JobRunExecutors;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Maintenance.System;
using AutomationSystem.Shared.Model;
using Moq;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using Xunit;

namespace AutomationSystem.Main.Tests.Maintenance.System.JobRunExecutors
{
    public class DatabaseClearingJobTests
    {
        private const long JobRunId = 55;
        private const string ReportParameterName = "DatabaseClearingJobReport";

        private readonly Mock<ICoreMaintenanceService> coreMaintenanceServiceMock;
        private readonly Mock<IMaintenanceDatabaseLayer> maintenanceDbMock;
        private readonly Mock<ICoreEmailService> coreEmailServiceMock;
        private readonly Mock<ITracerFactory> tracerFactoryMock;

        private readonly JobRun jobRun;
        private readonly DateTime expectedToDateFrom;
        private readonly DateTime expectedToDateTo;

        public DatabaseClearingJobTests()
        {
            coreMaintenanceServiceMock = new Mock<ICoreMaintenanceService>();
            maintenanceDbMock = new Mock<IMaintenanceDatabaseLayer>();
            coreEmailServiceMock = new Mock<ICoreEmailService>();

            tracerFactoryMock = new Mock<ITracerFactory>();
            tracerFactoryMock.Setup(x => x.CreateTracer<DatabaseClearingJob>(It.IsAny<object>())).Returns(Mock.Of<ITracer>());

            expectedToDateFrom = DateTime.UtcNow.AddMonths(-DatabaseClearingJob.MonthBeforeNowToDelete).AddMinutes(-5);
            expectedToDateTo = DateTime.UtcNow.AddMonths(-DatabaseClearingJob.MonthBeforeNowToDelete).AddMinutes(5);

            jobRun = new JobRun
            {
                JobRunId = JobRunId
            };
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
        public void Execute_NoIncidentAndChanges_NoReport()
        {
            // arrange
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.True(result.IsSuccess);
            coreEmailServiceMock.Verify(
                e => e.SendJobReportEmail(It.IsAny<EmailTypeEnum>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<Dictionary<string, object>>()),
                Times.Never);
        }

        [Fact]
        public void Execute_SomeClassCertificatesDeleted_SendReportWithInformation()
        {
            // arrange
            maintenanceDbMock.Setup(e => e.ClearClassCertificates(It.IsAny<DateTime>())).Returns(111);
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.True(result.IsSuccess);
            maintenanceDbMock.Verify(e => e.ClearClassCertificates(It.Is<DateTime>(x => expectedToDateFrom <= x && x <= expectedToDateTo)));
            VerifyEmailNotification("Certificates on classes: 111");
        }

        [Fact]
        public void Execute_SomeRegistrationCertificatesDeleted_SendReportWithInformation()
        {
            // arrange
            maintenanceDbMock.Setup(e => e.ClearClassRegistrationCertificates(It.IsAny<DateTime>())).Returns(222);
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.True(result.IsSuccess);
            maintenanceDbMock.Verify(e => e.ClearClassRegistrationCertificates(It.Is<DateTime>(x => expectedToDateFrom <= x && x <= expectedToDateTo)));
            VerifyEmailNotification("Certificates on registrations: 222");
        }

        [Fact]
        public void Execute_SomeClassMaterialFilesDeleted_SendReportWithInformation()
        {
            // arrange
            maintenanceDbMock.Setup(e => e.ClearClassMaterialFiles(It.IsAny<DateTime>())).Returns(333);
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.True(result.IsSuccess);
            maintenanceDbMock.Verify(e => e.ClearClassMaterialFiles(It.Is<DateTime>(x => expectedToDateFrom <= x && x <= expectedToDateTo)));
            VerifyEmailNotification("Unused materials on classes: 333");
        }

        [Fact]
        public void Execute_SomeFilesDeleted_SendReportWithInformation()
        {
            // arrange
            var expectedCount = DatabaseClearingJob.FileMaxItems * 3 - 1;
            coreMaintenanceServiceMock.SetupSequence(e => e.ClearDatabaseFiles(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DatabaseClearingJob.FileMaxItems)
                .Returns(DatabaseClearingJob.FileMaxItems)
                .Returns(DatabaseClearingJob.FileMaxItems - 1);
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.True(result.IsSuccess);
            coreMaintenanceServiceMock.Verify(
                e => e.ClearDatabaseFiles(
                    It.Is<DateTime>(x => expectedToDateFrom <= x && x <= expectedToDateTo),
                    DatabaseClearingJob.FileMaxItems),
                Times.Exactly(3));
            VerifyEmailNotification($"Unused files: {expectedCount}");
        }

        [Fact]
        public void Execute_ClearingFails_SendReportAndCreateIncident()
        {
            // arrange
            maintenanceDbMock.Setup(e => e.ClearClassCertificates(It.IsAny<DateTime>())).Throws<InvalidOperationException>();
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.False(result.IsSuccess);

            Assert.NotNull(result.Incident);
            AssertIncident(result.Incident, IncidentTypeEnum.MaintenanceError, EntityTypeEnum.CoreJobRun, JobRunId);

            var childIncident = result.Incident.Children.FirstOrDefault();
            Assert.NotNull(childIncident);
            AssertIncident(childIncident, IncidentTypeEnum.MaintenanceError, EntityTypeEnum.CoreJobRun, JobRunId);

            VerifyEmailNotification("Clearing of database causes error");
        }

        [Fact]
        public void Execute_ReportingFails_CreateIncident()
        {
            // arrange
            maintenanceDbMock.Setup(e => e.ClearClassMaterialFiles(It.IsAny<DateTime>())).Returns(1);
            coreEmailServiceMock.Setup(e => e.SendJobReportEmail(It.IsAny<EmailTypeEnum>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<Dictionary<string, object>>()))
                .Throws<InvalidOperationException>();
            var job = CreateJob();

            // act
            var result = job.Execute(jobRun);

            // assert
            Assert.False(result.IsSuccess);

            Assert.NotNull(result.Incident);
            AssertIncident(result.Incident, IncidentTypeEnum.MaintenanceError, EntityTypeEnum.CoreJobRun, JobRunId);

            var childIncident = result.Incident.Children.FirstOrDefault();
            Assert.NotNull(childIncident);
            AssertIncident(childIncident, IncidentTypeEnum.MaintenanceError, EntityTypeEnum.CoreJobRun, JobRunId);
        }

        #endregion

        #region private methods

        private void AssertIncident(IncidentForLog incident, IncidentTypeEnum incidentType, EntityTypeEnum? entityType, long? entityId)
        {
            Assert.Equal(incidentType, incident.IncidentTypeId);
            Assert.Equal(entityType, incident.EntityTypeId);
            Assert.Equal(entityId, incident.EntityId);
        }

        private void VerifyEmailNotification(string containsString)
        {
            coreEmailServiceMock.Verify(
                e => e.SendJobReportEmail(
                    EmailTypeEnum.DatabaseClearingJobReport,
                    (int)SeverityEnum.High,
                    JobRunId,
                    It.Is<Dictionary<string, object>>(
                        x => x[ReportParameterName].ToString().Contains(containsString))),
                Times.Once);
        }

        private DatabaseClearingJob CreateJob()
        {
            return new DatabaseClearingJob(
                coreMaintenanceServiceMock.Object,
                maintenanceDbMock.Object,
                coreEmailServiceMock.Object,
                tracerFactoryMock.Object);
        }

        #endregion
    }
}
