using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Maintenance.Data;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Contract.Maintenance.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.Maintenance.System.JobRunExecutors
{
    public class DatabaseClearingJobFactory : IJobRunExecutorFactory
    {
        private readonly ICoreMaintenanceService coreMaintenanceService;
        private readonly IMaintenanceDatabaseLayer maintenanceDb;
        private readonly ICoreEmailService coreEmailService;
        private readonly ITracerFactory tracerFactory;

        public DatabaseClearingJobFactory(
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

        public JobTypeEnum JobTypeId => JobTypeEnum.MainDatabaseClearingJob;

        public IJobRunExecutor CreateJobRunExecutor()
        {
            return new DatabaseClearingJob(coreMaintenanceService, maintenanceDb, coreEmailService, tracerFactory);
        }
    }
}
