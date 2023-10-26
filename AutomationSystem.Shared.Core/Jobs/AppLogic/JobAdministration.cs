using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Shared.Contract.Jobs.AppLogic;
using AutomationSystem.Shared.Contract.Jobs.AppLogic.Models;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Core.Jobs.Data;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Jobs.AppLogic
{
    public class JobAdministration : IJobAdministration
    {
        private readonly IJobDatabaseLayer jobDb;
        private readonly IJobScheduler jobScheduler;

        public JobAdministration(IJobDatabaseLayer jobDb, IJobScheduler jobScheduler)
        {
            this.jobDb = jobDb;
            this.jobScheduler = jobScheduler;
        }

        public List<JobListItem> GetJobListItems()
        {
            var activeJobs = jobDb.GetActiveJobs();
            var result = activeJobs.Select(MapToJobListItem).ToList();
            return result;
        }

        public long ScheduleJobRun(long jobId, DateTime plannedTime)
        {
            var result = jobScheduler.ScheduleJobRunForJob(jobId, plannedTime);
            return result;
        }

        #region private methods

        // todo: move to mapper;
        private JobListItem MapToJobListItem(Job job)
        {
            return new JobListItem
            {
                JobId = job.JobId,
                Name = job.Name,
                IntervalInMinutes = job.IntervalInMinutes,
                FromHourAndMinute = $"{job.FromHour:D2}:{job.FromMinute:D2}"
            };
        }

        #endregion
    }
}
