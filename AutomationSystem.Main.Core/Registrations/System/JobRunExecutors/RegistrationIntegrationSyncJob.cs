using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.Integration.System;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Contract.Jobs.System.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Classes.System.Emails;

namespace AutomationSystem.Main.Core.Registrations.System.JobRunExecutors
{
    /// <summary>
    /// Job that synchronize registration with outer integration system
    /// </summary>
    public class RegistrationIntegrationSyncJob : IJobRunExecutor
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly ITracerFactory tracerFactory;
        private readonly IClassEmailService classEmailService;

            private readonly Dictionary<IntegrationTypeEnum, IRegistrationIntegrationSyncExecutorFactory> executorFactories;

        private ITracer tracer;

        public bool CanReportIncident => true;

        public RegistrationIntegrationSyncJob(
            IClassDatabaseLayer classDb,
            IRegistrationDatabaseLayer registrationDb,
            IEnumerable<IRegistrationIntegrationSyncExecutorFactory> syncExecutorFactories,
            ITracerFactory tracerFactory,
            IClassEmailService classEmailService)
        {
            this.classDb = classDb;
            this.registrationDb = registrationDb;
            this.tracerFactory = tracerFactory;
            this.classEmailService = classEmailService;

            executorFactories = new Dictionary<IntegrationTypeEnum, IRegistrationIntegrationSyncExecutorFactory>();
            RegisterRegistrationIntegrationSyncExecutorFactories(syncExecutorFactories);
        }

        public JobRunExecutorResult Execute(JobRun jobRun)
        {
            tracer = tracerFactory.CreateTracer<RegistrationIntegrationSyncJob>(jobRun.JobRunId);

            // loads active classes
            var nowUtc = DateTime.UtcNow;
            var filter = new ClassFilter
            {
                OnlyInAfterRegistration = true,
                EventEndUtcFrom = nowUtc
            };
            var activeClasses = classDb.GetClassesByFilter(filter, ClassIncludes.ClassType);
            tracer.Info($"Active classes was loaded: {string.Join(",", activeClasses.Select(x => x.ClassId))}.");

            // process all classes
            var incidents = new List<IncidentForLog>();
            foreach (var cls in activeClasses)
            {
                try
                {
                    tracer.Info($"Processing of class {cls.ClassId}");
                    
                    // finds executor by class integration type and executes synchronisation
                    if (!executorFactories.TryGetValue(cls.IntegrationTypeId, out var executorFactory))
                        throw new InvalidOperationException($"There is no sync executor factory for integration type {cls.IntegrationTypeId}.");

                    // loads registrations
                    var registrationFilter = new RegistrationFilter { IsApproved = true, ClassId = cls.ClassId };
                    registrationFilter.ExcludedRegistrationTypeIds.Add(RegistrationTypeEnum.WWA);
                    var registrations = registrationDb.GetRegistrationsByFilter(registrationFilter, ClassRegistrationIncludes.AddressesCountry);

                    // creates executor and executes sync
                    var executor = executorFactory.CreateRegistrationIntegrationSyncExecutor();
                    var syncResult = executor.ExecuteSync(jobRun, cls, registrations);

                    // process and logs the result of sync
                    incidents.AddRange(syncResult.Incidents);
                    if (syncResult.Incidents.Count == 0)
                        tracer.Info($"Synchronisation of class {cls.ClassId} was successful.");
                    else
                        tracer.Warning($"Synchronisation of class {cls.ClassId} failed. There are {syncResult.Incidents.Count} incidents");
                    
                    // sends report email to coordinator     
                    if (syncResult.SendReport)
                    {
                        var textMap = new Dictionary<string, object>();
                        textMap["OuterSystemSyncJobReport"] = syncResult.ReportContent;
                        classEmailService.SendClassTextMapEmailByTypeToRecipient(EmailTypeEnum.OuterSystemSyncJobReport,
                            cls.ClassId, textMap, RecipientType.Coordinator);
                        tracer.Info($"Report email was processed for class {cls.ClassId}.");
                    }
                }
                catch (Exception e)
                {
                    tracer.Error(e, "Class synchronisation causes error.");
                    var incident = IncidentForLog
                        .New(IncidentTypeEnum.JobRunError, e)
                        .Entity(EntityTypeEnum.MainClass, cls.ClassId);
                    incidents.Add(incident);
                }
            }

            // process incidents to result
            var result = new JobRunExecutorResult();
            if (incidents.Count > 0)
            {
                result.IsSuccess = false;
                result.Incident = IncidentForLog
                    .New(IncidentTypeEnum.JobRunError, "RegistrationIntegrationSyncJob encounters problems", "See inner incidents...")
                    .Entity(EntityTypeEnum.CoreJobRun, jobRun.JobId)
                    .AddChildrenCollection(incidents);
            }
            return result;
        }

        #region private methods

        private void RegisterRegistrationIntegrationSyncExecutorFactories(IEnumerable<IRegistrationIntegrationSyncExecutorFactory> syncExecutorFactories)
        {
            foreach (var syncExecutorFactory in syncExecutorFactories)
            {
                executorFactories.Add(syncExecutorFactory.IntegrationTypeId, syncExecutorFactory);
            }
        }

        #endregion
    }
}
