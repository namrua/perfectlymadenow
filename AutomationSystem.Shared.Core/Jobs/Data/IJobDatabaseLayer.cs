using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Jobs.Data
{
    /// <summary>
    /// Job database layer
    /// </summary>
    public interface IJobDatabaseLayer
    {

        #region scheduler

        Job GetActiveJobById(long jobId);

        // gets active jobs
        List<Job> GetActiveJobs();

        // gets new job runs in timespan
        List<JobRun> GetNewJobRunsInTimespan(DateTime? from, DateTime? to, bool includeJob = false);

        long InsertJobRun(JobRun jobRun);

        // inserts job runs
        void InsertJobRuns(IEnumerable<JobRun> jobRuns);

        #endregion


        #region executor

        // gets all job runs to execute, new runs before last run are set as skipped
        List<JobRun> GetJobRunsToExecuteAndSkipRest(DateTime to);

        // starts new job run
        void StartJobRun(long jobRunId);

        // finish job run
        void FinishJobRun(long jobRunId, ProcessingStateEnum teminationState, string jsonResult);

        #endregion
    }

}
