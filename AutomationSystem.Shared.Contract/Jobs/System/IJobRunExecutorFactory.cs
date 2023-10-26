using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Jobs.System
{
    /// <summary>
    /// Factory for job run executor
    /// </summary>
    public interface IJobRunExecutorFactory
    {
        JobTypeEnum JobTypeId { get; }
        IJobRunExecutor CreateJobRunExecutor();
    }
   
}
