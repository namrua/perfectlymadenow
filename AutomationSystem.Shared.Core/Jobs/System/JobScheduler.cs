using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Core.Jobs.Data;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Jobs.System
{
    /// <summary>
    /// Schedules job runs
    /// </summary>
    public class JobScheduler : IJobScheduler
    {
        private const int minSpaceInMinutes = 1;

        private readonly IJobDatabaseLayer jobDb;

        public JobScheduler(IJobDatabaseLayer jobDb)
        {
            this.jobDb = jobDb;
        }

        public long ScheduleJobRunForJob(long jobId, DateTime plannedTime)
        {
            var job = jobDb.GetActiveJobById(jobId);
            if (job == null)
            {
                throw new ArgumentException($"There is no job with id {jobId}.");
            }

            var jobRun = CreateJobRun(job, plannedTime);
            var jobRunId = jobDb.InsertJobRun(jobRun);
            return jobRunId;
        }

        public List<long> ScheduleJobRuns(DateTime from, DateTime to)
        {
            // loads existing jobs and job runs
            var activeJobs = jobDb.GetActiveJobs();
            var jobRuns = jobDb.GetNewJobRunsInTimespan(from, to);
            var groupedJobRunMap = jobRuns.GroupBy(x => x.JobId)
                .ToDictionary(x => x.Key, y => new Stack<DateTime>(y.Select(p => p.Planned).OrderByDescending(p => p)));

            // creates new job runs for each job
            var toInsert = new List<JobRun>();
            foreach (var job in activeJobs)
            {
                // get stack with existing jobs
                Stack<DateTime> existingJobRuns;
                if (!groupedJobRunMap.TryGetValue(job.JobId, out existingJobRuns))
                    existingJobRuns = new Stack<DateTime>();

                // computes new jobs
                toInsert.AddRange(GetNewJobRuns(from, to, job, existingJobRuns));
            }
            
            // adds new job runs
            jobDb.InsertJobRuns(toInsert);
            var result = toInsert.Select(x => x.JobRunId).ToList();
            return result;
        }


        #region private methods

        // creates new jobs
        private List<JobRun> GetNewJobRuns(DateTime from, DateTime to, Job job, Stack<DateTime> existingJobRuns)
        {
            // computes start time;
            var interval = job.IntervalInMinutes;
            var seedTime = new DateTime(from.Year, from.Month, from.Day, job.FromHour, job.FromMinute, 0);
            var plannedTime = new DateTime(from.Year, from.Month, from.Day, from.Hour, from.Minute, 0);
            var offset = ((int)(plannedTime - seedTime).TotalMinutes) % interval;                       
            plannedTime = plannedTime.AddMinutes(-offset);

            // creates jobs
            var result = new List<JobRun>();
            DateTime? existingRun = null;
            for (; plannedTime < to; plannedTime = plannedTime.AddMinutes(interval))
            {
                // computes closest existingRun dateTime
                var plannedSpaceStart = plannedTime.AddMinutes(-minSpaceInMinutes);
                while (existingJobRuns.Count > 0)
                {
                    if (!existingRun.HasValue || existingRun.Value <= plannedSpaceStart)
                        existingRun = existingJobRuns.Pop();
                    if (plannedSpaceStart <= existingRun)
                        break;                    
                }              

                // filters used or not in interval planned times
                if (plannedTime < from) continue;                
                if (existingRun.HasValue
                    && existingRun.Value.AddMinutes(-minSpaceInMinutes) < plannedTime
                    && plannedTime < existingRun.Value.AddMinutes(minSpaceInMinutes))
                    continue;

                // adds planned time
                result.Add(CreateJobRun(job, plannedTime));
            }
            return result;
        }

        // creates JobRun
        private JobRun CreateJobRun(Job job, DateTime plannedTime)
        {
            var result = new JobRun
            {
                Active = true,
                JobId = job.JobId,
                Planned = plannedTime,
                ProcessingStateId = ProcessingStateEnum.New,
                Severity = job.Severity
            };
            return result;
        }

        #endregion
    }
}
