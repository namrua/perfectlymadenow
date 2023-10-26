using System;
using System.Configuration;
using System.IO;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Contract.Jobs.System.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Jobs.System.JobRunExecutors
{
    /// <summary>
    /// Serves for testing of JobRunsEnvelope
    /// </summary>
    public class TestJobRunExecutor : IJobRunExecutor 
    {
        private readonly ITracerFactory tracerFactory;

        private ITracer tracer;

        // constructor
        public TestJobRunExecutor(ITracerFactory tracerFactory)
        {
            this.tracerFactory = tracerFactory;
        }

        // determines whether request incidents can be reported
        public bool CanReportIncident => false;

        // executes job run
        public JobRunExecutorResult Execute(JobRun jobRun)
        {
            tracer = tracerFactory.CreateTracer<TestJobRunExecutor>(jobRun.JobRunId);
            tracer.Info($"Executed on {DateTime.Now}");

            CheckWebJobAppDataAccess();

            return new JobRunExecutorResult();
        }

        // checks web job access file
        private void CheckWebJobAppDataAccess()
        {                       
            try
            {
                var rootPath = ConfigurationManager.AppSettings["WebRootPath"];
                var path = Path.Combine(rootPath, @"App_Data\WebJobAccessTest.txt");
                tracer.Info($"App_Data path: {path}");

                using (var fs = new FileStream(path, FileMode.Open))
                using (var sr = new StreamReader(fs))
                {
                    var result = sr.ReadToEnd();
                    tracer.Info($"Check App_Data accessibility: {result}");
                }                
            }
            catch (Exception e)
            {
                tracer.Error(e, "Cannot access App_Data folder.");               
            }           
        }
    }
}
