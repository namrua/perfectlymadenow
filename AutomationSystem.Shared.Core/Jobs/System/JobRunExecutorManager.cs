using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Contract.Jobs.System.Models;
using AutomationSystem.Shared.Core.Jobs.Data;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Shared.Core.Jobs.System
{
    /// <summary>
    /// Executes job runs
    /// </summary>
    public class JobRunExecutorManager : IJobRunExecutorManager
    {
        // private fields
        private readonly Dictionary<JobTypeEnum, IJobRunExecutorFactory> jobRunFactories;
        private readonly IIncidentLogger incidentLogger;
        private readonly ITracerFactory tracerFactory;
        private readonly ITracer tracer;

        // private components
        private readonly IJobDatabaseLayer jobDb;


        // constructor
        public JobRunExecutorManager(
            IJobDatabaseLayer jobDb,
            IIncidentLogger incidentLogger,
            IEnumerable<IJobRunExecutorFactory> jobRunExecutorFactories,
            ITracerFactory tracerFactory)
        {
            this.incidentLogger = incidentLogger;
            this.tracerFactory = tracerFactory;
            this.jobDb = jobDb;

            tracer = tracerFactory.CreateTracer<JobRunExecutorManager>();
            jobRunFactories = new Dictionary<JobTypeEnum, IJobRunExecutorFactory>();
            RegisterJobRunExecutorFactories(jobRunExecutorFactories);
        }

        // process all runs
        public void ExecuteAllJobRuns(DateTime to)
        {
            // loads jobs
            var jobRunsToExcecute = jobDb.GetJobRunsToExecuteAndSkipRest(to);
            var jobCount = jobRunsToExcecute.Count;
            tracer.Info($"There was {jobCount} planned job runs to be executed");
            if (jobCount == 0) return;
       
            // restarts caches            
            CacheRepository.ExpireAllCaches();
            tracer.Info("All caches was set to expired");

            // executes jobs
            foreach (var jobRun in jobRunsToExcecute.OrderBy(x => x.Severity))
                ExecuteJobRun(jobRun);
            tracer.Info("All planned job runs was executed");            
        }

        #region private methods

        // executes one job run
        private void ExecuteJobRun(JobRun jobRun)
        {
            var runTracer = tracerFactory.CreateTracer<JobRunExecutorManager>(jobRun.JobRunId);
            runTracer.Info("Execution of job run started");

            var result = new JobRunExecutorResult();
            IJobRunExecutor executor = null;
            try
            {
                // starts job
                jobDb.StartJobRun(jobRun.JobRunId);
                runTracer.Info("Job run was updated as started");

                // loads executor                
                var type = jobRun.Job.JobTypeId;
                if (!jobRunFactories.TryGetValue(type, out var jobRunFactory))
                    throw new ArgumentException($"There is no registered JobRunExecutorFactory with type {type}");
                executor = jobRunFactory.CreateJobRunExecutor();
                runTracer.Info($"Executor was created for job type {jobRun.Job.JobTypeId}");

                // executes job run                
                result = executor.Execute(jobRun);
                runTracer.Info($"Job run was executed: isSuccess = {result.IsSuccess}");
            }
            catch (Exception e)
            {
                runTracer.Error(e, "Execution of job run caused exception");
                result = new JobRunExecutorResult(
                    IncidentForLog.New(IncidentTypeEnum.JobRunError, e)
                        .Entity(EntityTypeEnum.CoreJobRun, jobRun.JobRunId));
            }
            finally
            {
                try
                {
                    if (result.IsSuccess)
                    {
                        jobDb.FinishJobRun(jobRun.JobRunId, ProcessingStateEnum.Finished, result.JsonResult);
                        runTracer.Info("Job run was sucessfully finished");
                    }
                    else
                    {
                        jobDb.FinishJobRun(jobRun.JobRunId, ProcessingStateEnum.Error, result.JsonResult);     
                        runTracer.Warning("Job run was terminated with error");
                        if (result.Incident != null)
                        {
                            if (executor != null && !executor.CanReportIncident)
                                result.Incident.NonReportable();
                            incidentLogger.LogIncident(result.Incident);
                        }
                    }
                }
                catch (Exception e)
                {
                    runTracer.Error(e, "Finally block of ExecutionJobRun method caused exception");
                }
            }
        }

        // registers job run executor factories
        private void RegisterJobRunExecutorFactories(IEnumerable<IJobRunExecutorFactory> jobRunExecutorFactories)
        {
            foreach (var jobRunExecutorFactory in jobRunExecutorFactories)
            {
                jobRunFactories[jobRunExecutorFactory.JobTypeId] = jobRunExecutorFactory;
            }
        }

        #endregion
    }

}
