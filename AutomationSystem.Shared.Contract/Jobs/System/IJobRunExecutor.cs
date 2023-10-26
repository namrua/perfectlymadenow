using AutomationSystem.Shared.Contract.Jobs.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Jobs.System
{
    /// <summary>
    /// Executes job run
    /// </summary>
    public interface IJobRunExecutor
    {

        // determines whether request incidents can be reported
        bool CanReportIncident { get; }

        // executes job run
        JobRunExecutorResult Execute(JobRun jobRun);

    }
   
}
