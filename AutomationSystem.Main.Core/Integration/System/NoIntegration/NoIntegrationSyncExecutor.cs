using System.Collections.Generic;
using AutomationSystem.Main.Core.Integration.System.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.Integration.System.NoIntegration
{
    /// <summary>
    /// Executes registration integration synchronisation for no integration
    /// </summary>
    public class NoIntegrationSyncExecutor : IRegistrationIntegrationSyncExecutor
    {
        private readonly ITracerFactory traceFactory;

        public NoIntegrationSyncExecutor(ITracerFactory traceFactory)
        {
            this.traceFactory = traceFactory;
        }

        public RegistrationIntegrationSyncResult ExecuteSync(JobRun jobRun, Class cls, List<ClassRegistration> approvedClosedRegistrations)
        {
            var tracer = traceFactory.CreateTracer<NoIntegrationSyncExecutor>(jobRun.JobRunId);
            tracer.Info("Class has no integration.");
            return new RegistrationIntegrationSyncResult { SendReport = false };
        }
    }
}
