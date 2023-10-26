using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.AsyncRequests.System
{
    /// <summary>
    /// Converts async request entitites
    /// </summary>
    public interface IAsyncRequestConvertor
    {

        // converts async request to async request detail
        AsyncRequestDetail ConvertToAsyncRequestDetail(AsyncRequest request);

    }
}
