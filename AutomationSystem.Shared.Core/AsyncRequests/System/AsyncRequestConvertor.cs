using System;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.AsyncRequests.System
{
    /// <summary>
    /// Converts async request entitites
    /// </summary>
    public class AsyncRequestConvertor : IAsyncRequestConvertor
    {

        // converts async request to async request detail
        public AsyncRequestDetail ConvertToAsyncRequestDetail(AsyncRequest request)
        {
            if (request.ProcessingState == null)
                throw new InvalidOperationException("ProcessingState is not included into AsyncRequest object.");
            if (request.AsyncRequestType == null)
                throw new InvalidOperationException("AsyncRequestType is not included into AsyncRequest object.");

            var result = new AsyncRequestDetail
            {
                AsyncRequestId = request.AsyncRequestId,
                AsyncRequestTypeId = request.AsyncRequestTypeId,
                AsyncRequestType = request.AsyncRequestType.Description,
                ProcessingStateId = request.ProcessingStateId,
                ProcessingState = request.ProcessingState.Description,
                Planned = request.Planned,
                Started = request.Started,
                Finished = request.Finished
            };
            return result;
        }

    }

}
