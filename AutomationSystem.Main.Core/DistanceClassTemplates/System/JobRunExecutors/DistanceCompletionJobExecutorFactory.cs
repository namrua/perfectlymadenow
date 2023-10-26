using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Jobs.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.System.JobRunExecutors
{
    public class DistanceCompletionJobExecutorFactory : IJobRunExecutorFactory
    {
        private readonly IDistanceClassTemplateService distanceTemplateService;
        private readonly IDistanceClassTemplateDatabaseLayer distanceTemplateDb;
        private readonly ICoreEmailService coreEmailService;
        private readonly ITracerFactory tracerFactory;

        public DistanceCompletionJobExecutorFactory(
            IDistanceClassTemplateService distanceTemplateService,
            IDistanceClassTemplateDatabaseLayer distanceTemplateDb,
            ICoreEmailService coreEmailService,
            ITracerFactory tracerFactory)
        {
            this.distanceTemplateService = distanceTemplateService;
            this.distanceTemplateDb = distanceTemplateDb;
            this.coreEmailService = coreEmailService;
            this.tracerFactory = tracerFactory;
        }

        public JobTypeEnum JobTypeId => JobTypeEnum.MainDistanceClassCompletionJob;

        public IJobRunExecutor CreateJobRunExecutor()
        {
            return new DistanceCompletionJobExecutor(
                distanceTemplateService,
                distanceTemplateDb,
                coreEmailService,
                tracerFactory);
        }
    }
}
