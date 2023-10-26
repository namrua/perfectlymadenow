using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Maintenance.Data;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Contract.Jobs.System.Models;
using AutomationSystem.Shared.Contract.Maintenance.System;
using AutomationSystem.Shared.Model;
using CorabeuControl.Helpers;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.Maintenance.System.JobRunExecutors
{
    public class DatabaseClearingJob : IJobRunExecutor
    {
        public const int MonthBeforeNowToDelete = 3;
        public const int FileMaxItems = 10;

        private readonly ICoreMaintenanceService coreMaintenanceService;
        private readonly IMaintenanceDatabaseLayer maintenanceDb;
        private readonly ICoreEmailService coreEmailService;
        private readonly ITracerFactory tracerFactory;

        private ITracer tracer;
        private StringBuilder reportContent;
        private List<IncidentForLog> incidents;
        private JobRun currentJobRun;
        private int totalDeleted;

        public DatabaseClearingJob(
            ICoreMaintenanceService coreMaintenanceService,
            IMaintenanceDatabaseLayer maintenanceDb,
            ICoreEmailService coreEmailService,
            ITracerFactory tracerFactory)
        {
            this.coreMaintenanceService = coreMaintenanceService;
            this.maintenanceDb = maintenanceDb;
            this.coreEmailService = coreEmailService;
            this.tracerFactory = tracerFactory;
        }

        public bool CanReportIncident => true;

        public JobRunExecutorResult Execute(JobRun jobRun)
        {
            totalDeleted = 0;
            currentJobRun = jobRun;
            tracer = tracerFactory.CreateTracer<DatabaseClearingJob>(jobRun.JobRunId);
            incidents = new List<IncidentForLog>();
            reportContent = new StringBuilder();

            var toDate = DateTime.UtcNow.AddMonths(-MonthBeforeNowToDelete);
            tracer.Info($"Clearing database to date {toDate}");
            reportContent.AppendLine($"Following items was removed from database (all older than {TextHelper.GetStringDate(toDate)}):");
            try
            {
                var countOfDeleted = maintenanceDb.ClearClassCertificates(toDate);
                ReportClearing(countOfDeleted, "Certificates on classes");

                countOfDeleted = maintenanceDb.ClearClassRegistrationCertificates(toDate);
                ReportClearing(countOfDeleted, "Certificates on registrations");

                countOfDeleted = maintenanceDb.ClearClassMaterialFiles(toDate);
                ReportClearing(countOfDeleted, "Unused materials on classes");

                countOfDeleted = 0;
                int deletedFileCount;
                do
                {
                    deletedFileCount = coreMaintenanceService.ClearDatabaseFiles(toDate, FileMaxItems);
                    countOfDeleted += deletedFileCount;

                } while (deletedFileCount >= FileMaxItems);
                ReportClearing(countOfDeleted, "Unused files");

                reportContent.Append("Clearing of database was SUCCESSFUL.");
                tracer.Info("Clearing database was successful");
            }
            catch (Exception e)
            {
               AddIncident(e, "Clearing of database causes error.");
               reportContent.Append($"Clearing of database causes error:\n{e}");
            }

            SendReportEmail();

            var result = CreateJobRunExecutorResult();
            return result;
        }

        #region private methods

        private void ReportClearing(int countOfDeleted, string item)
        {
            totalDeleted += countOfDeleted;

            if (countOfDeleted != 0)
            {
                reportContent.AppendLine($"- {item}: {countOfDeleted}");
            }

            tracer.Info($"{countOfDeleted} {item} was deleted.");
        }

        private void SendReportEmail()
        {
            if (totalDeleted == 0 && !incidents.Any())
            {
                return;
            }

            try
            {
                var textMap = new Dictionary<string, object>
                {
                    ["DatabaseClearingJobReport"] = reportContent.ToString()
                };
                coreEmailService.SendJobReportEmail(EmailTypeEnum.DatabaseClearingJobReport, (int) SeverityEnum.High, currentJobRun.JobRunId, textMap);
                tracer.Info("Report email was processed.");
            }
            catch (Exception e)
            {
                AddIncident(e, "Sending of report causes error.");
            }
        }

        private void AddIncident(Exception e, string message)
        {
            tracer.Error(e, message);
            var incident = IncidentForLog
                .New(IncidentTypeEnum.MaintenanceError, e)
                .Entity(EntityTypeEnum.CoreJobRun, currentJobRun.JobRunId);
            incidents.Add(incident);
        }

        private JobRunExecutorResult CreateJobRunExecutorResult()
        {
            var result = new JobRunExecutorResult();
            if (incidents.Count > 0)
            {
                result.IsSuccess = false;
                result.Incident = IncidentForLog
                    .New(IncidentTypeEnum.MaintenanceError, "ClearDatabaseJob encounters problems", "See inner incidents...")
                    .Entity(EntityTypeEnum.CoreJobRun, currentJobRun.JobRunId)
                    .AddChildrenCollection(incidents);
            }
            return result;
        }

        #endregion
    }
}
