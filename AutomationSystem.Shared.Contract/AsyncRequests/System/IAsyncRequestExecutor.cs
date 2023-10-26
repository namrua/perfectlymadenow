using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.AsyncRequests.System
{
    /// <summary>
    /// Executes one async request
    /// </summary>
    public interface IAsyncRequestExecutor
    {
        
        // determines whether request incidents can be reported
        bool CanReportIncident { get; }        

        // executes task, returns json result
        AsyncRequestExecutorResult Execute(AsyncRequest request);

    }
}
