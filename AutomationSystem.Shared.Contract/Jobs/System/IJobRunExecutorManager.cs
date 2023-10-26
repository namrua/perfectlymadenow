using System;

namespace AutomationSystem.Shared.Contract.Jobs.System
{
    /// <summary>
    /// Executes job runs
    /// </summary>
    public interface IJobRunExecutorManager
    {
        // process all runs
        void ExecuteAllJobRuns(DateTime to);
    }
   
}
