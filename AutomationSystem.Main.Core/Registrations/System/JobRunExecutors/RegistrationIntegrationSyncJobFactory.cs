using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Integration.System;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Shared.Contract.Jobs.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Classes.System.Emails;

namespace AutomationSystem.Main.Core.Registrations.System.JobRunExecutors
{
    /// <summary>
    /// Factory for RegistrationIntegrationSyncJob
    /// </summary>
    public class RegistrationIntegrationSyncJobFactory : IJobRunExecutorFactory
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IEnumerable<IRegistrationIntegrationSyncExecutorFactory> syncExecutorFactories;
        private readonly ITracerFactory tracerFactory;
        private readonly IClassEmailService classEmailService;

        public JobTypeEnum JobTypeId => JobTypeEnum.OuterSystemSyncJob;

        public RegistrationIntegrationSyncJobFactory(
            IClassDatabaseLayer classDb,
            IRegistrationDatabaseLayer registrationDb,
            IEnumerable<IRegistrationIntegrationSyncExecutorFactory> syncExecutorFactories,
            ITracerFactory tracerFactory,
            IClassEmailService classEmailService)
        {
            this.classDb = classDb;
            this.registrationDb = registrationDb;
            this.syncExecutorFactories = syncExecutorFactories;
            this.tracerFactory = tracerFactory;
            this.classEmailService = classEmailService;
        }

        public IJobRunExecutor CreateJobRunExecutor()
        {
            return new RegistrationIntegrationSyncJob(classDb, registrationDb, syncExecutorFactories, tracerFactory, classEmailService);
        }
    }
}
