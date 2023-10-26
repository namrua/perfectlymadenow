using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Jobs.Data
{
    /// <summary>
    /// Job database layer
    /// </summary>
    public class JobDatabaseLayer : IJobDatabaseLayer
    {
        #region scheduler

        public Job GetActiveJobById(long jobId)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Jobs.Active().FirstOrDefault(x => x.JobId == jobId && x.Active);
                return result;
            }
        }


        // gets active jobs
        public List<Job> GetActiveJobs()
        {
            using (var context = new CoreEntities())
            {
                var result = context.Jobs.Active().Where(x => x.Active).ToList();
                return result;
            }
        }


        // gets new job runs in timespan
        public List<JobRun> GetNewJobRunsInTimespan(DateTime? from, DateTime? to, bool includeJob = false)
        {
            using (var context = new CoreEntities())
            {
                DbQuery<JobRun> queryBase = context.JobRuns;
                if (includeJob)
                    queryBase = queryBase.Include("Job");
                var query = queryBase.Active().Where(x => x.ProcessingStateId == ProcessingStateEnum.New && x.Active && x.Job.Active);
                if (from.HasValue)
                    query = query.Where(x => from.Value <= x.Planned);
                if (to.HasValue)
                    query = query.Where(x => x.Planned < to.Value);
                var result = query.ToList();
                return result;
            }
        }

        public long InsertJobRun(JobRun jobRun)
        {
            using (var context = new CoreEntities())
            {
                context.JobRuns.Add(jobRun);
                context.SaveChanges();
                return jobRun.JobRunId;
            }
        }

        // inserts job runs
        public void InsertJobRuns(IEnumerable<JobRun> jobRuns)
        {
            using (var context = new CoreEntities())
            {
                context.JobRuns.AddRange(jobRuns);
                context.SaveChanges();
            }
        }

        #endregion


        #region executor   

        // gets all job runs to execute, new runs before last run are set as skipped
        public List<JobRun> GetJobRunsToExecuteAndSkipRest(DateTime to)
        {
            using (var context = new CoreEntities())
            {
                // loads job runs
                var runs = context.JobRuns.Include("Job").Active()
                    .Where(x => x.ProcessingStateId == ProcessingStateEnum.New && x.Active && x.Job.Active && x.Planned <= to).ToList();                   
                
                // categorize runs
                var result = new List<JobRun>();
                var toSkip = new List<JobRun>();
                foreach (var runsOfJob in runs.GroupBy(x => x.JobId))
                {
                    var sorted = runsOfJob.OrderByDescending(x => x.Planned);
                    result.Add(sorted.First());
                    toSkip.AddRange(sorted.Skip(1));
                }

                // sets skipped runs as skipped
                foreach(var runToSkip in toSkip)
                    runToSkip.ProcessingStateId = ProcessingStateEnum.Skipped;
                context.SaveChanges();

                // return result
                return result;
            }           
        }

        // starts new job run
        public void StartJobRun(long jobRunId)
        {
            using (var context = new CoreEntities())
            {
                var jobRun = context.JobRuns.Active().FirstOrDefault(x => x.JobRunId == jobRunId 
                    && x.ProcessingStateId == ProcessingStateEnum.New && x.Active && x.Job.Active);
                if (jobRun == null)
                    throw new ArgumentException($"There is no Job run with id {jobRunId}, or it is in wrong unexpected state.");
                jobRun.ProcessingStateId = ProcessingStateEnum.InProcess;
                jobRun.Started = DateTime.Now;
                context.SaveChanges();                
            }
        }

        // finish job run
        public void FinishJobRun(long jobRunId, ProcessingStateEnum teminationState, string jsonResult)
        {
            using (var context = new CoreEntities())
            {
                var jobRun = context.JobRuns.Active().FirstOrDefault(x => x.JobRunId == jobRunId);
                if (jobRun == null)
                    throw new ArgumentException($"There is no Job run with id {jobRunId}.");
                jobRun.ProcessingStateId = teminationState;
                jobRun.Finished = DateTime.Now;
                jobRun.JsonResult = jsonResult;
                context.SaveChanges();
            }
        }

        #endregion

    }

}
