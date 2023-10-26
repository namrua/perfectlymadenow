using System;
using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Jobs.System
{
    /// <summary>
    /// Job manager serves to scheduling job runs
    /// </summary>
    public interface IJobScheduler
    {
        long ScheduleJobRunForJob(long jobId, DateTime plannedTime);

        List<long> ScheduleJobRuns(DateTime from, DateTime to);
    }
}
