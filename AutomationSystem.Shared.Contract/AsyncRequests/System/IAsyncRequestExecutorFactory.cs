using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.AsyncRequests.System
{
    /// <summary>
    /// Factory for async request executors
    /// </summary>
    public interface IAsyncRequestExecutorFactory
    {
        HashSet<AsyncRequestTypeEnum> SupportedAsyncRequestTypes { get; }
        IAsyncRequestExecutor CreateAsyncRequestExecutor();
    }
}
