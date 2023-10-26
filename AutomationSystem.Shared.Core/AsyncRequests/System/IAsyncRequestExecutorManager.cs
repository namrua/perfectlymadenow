using System.Threading.Tasks;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;

namespace AutomationSystem.Shared.Core.AsyncRequests.System
{
    /// <summary>
    /// Manages async executors
    /// </summary>
    public interface IAsyncRequestExecutorManager
    {
        // executes request synchronously
        AsyncRequestExecutorResult Execute(long asyncRequestId);

        // executes request asynchronously
        Task<AsyncRequestExecutorResult> ExecuteAsync(long asyncRequestId);
    }
}
