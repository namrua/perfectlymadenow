using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Jobs.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Jobs.System.JobRunExecutors
{
    /// <summary>
    /// Factory for TestJobRunExecutor
    /// </summary>
    public class TestJobRunExecutorFactory : IJobRunExecutorFactory
    {
        private readonly ITracerFactory tracerFactory;

        public JobTypeEnum JobTypeId => JobTypeEnum.TestJob;

        public TestJobRunExecutorFactory(ITracerFactory tracerFactory)
        {
            this.tracerFactory = tracerFactory;
        }

        public IJobRunExecutor CreateJobRunExecutor()
        {
            return new TestJobRunExecutor(tracerFactory);
        }
    }

}
