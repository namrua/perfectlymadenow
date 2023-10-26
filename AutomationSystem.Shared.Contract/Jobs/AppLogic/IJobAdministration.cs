using System;
using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Jobs.AppLogic.Models;

namespace AutomationSystem.Shared.Contract.Jobs.AppLogic
{
    public interface IJobAdministration
    {
        List<JobListItem> GetJobListItems();

        long ScheduleJobRun(long jobId, DateTime plannedTime);
    }
}
