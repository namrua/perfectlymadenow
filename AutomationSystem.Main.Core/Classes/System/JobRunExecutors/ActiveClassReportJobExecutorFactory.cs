using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.System.Emails;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.Reports.System;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Jobs.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.Classes.System.JobRunExecutors
{
    /// <summary>
    /// Factory for ActiveClassReportJobExecutor
    /// </summary>
    public class ActiveClassReportJobExecutorFactory : IJobRunExecutorFactory
    {
        private readonly IReportService reportService;
        private readonly IClassDatabaseLayer classDb;
        private readonly IMainFileService mainFileService;
        private readonly IEmailAttachmentProviderFactory emailAttachmentProviderFactory;
        private readonly ITracerFactory tracerFactory;
        private readonly IClassEmailService classEmailService;

        public JobTypeEnum JobTypeId => JobTypeEnum.MainActiveClassReportJob;

        public ActiveClassReportJobExecutorFactory(
            IReportService reportService,
            IClassDatabaseLayer classDb,
            IMainFileService mainFileService,
            IEmailAttachmentProviderFactory emailAttachmentProviderFactory,
            ITracerFactory tracerFactory,
            IClassEmailService classEmailService)
        {
            this.reportService = reportService;
            this.classDb = classDb;
            this.mainFileService = mainFileService;
            this.emailAttachmentProviderFactory = emailAttachmentProviderFactory;
            this.tracerFactory = tracerFactory;
            this.classEmailService = classEmailService;
        }

        public IJobRunExecutor CreateJobRunExecutor()
        {
            return new ActiveClassReportJobExecutor(
                reportService,
                classDb,
                mainFileService,
                emailAttachmentProviderFactory,
                tracerFactory,
                classEmailService);
        }
    }
}
