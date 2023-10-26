using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Core.AsyncRequests.Data.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.AsyncRequests.Data
{
    /// <summary>
    /// Asynchronous Request and Jobs database layer
    /// </summary>
    public interface IAsyncDatabaseLayer
    {

        // gets async request by entity and type
        List<AsyncRequest> GetAsyncRequestsByEntityAndType(EntityTypeEnum? entityTypeId, long? entityId,
            IEnumerable<AsyncRequestTypeEnum> types, AsyncRequestIncludes includes = AsyncRequestIncludes.None);
       

        // inserts new async request
        AsyncRequest InsertAsyncRequest(AsyncRequestTypeEnum type, EntityTypeEnum? entityType, long? entityId, int severity, string jsonParameter = null);

        // loads async request and set it to InProcess when it is active and new
        AsyncRequest GetAsyncRequestToProcessing(long asyncRequestId, out bool wasStarted);

        // sets async request state
        void FinishAsyncRequest(long asyncRequestId, ProcessingStateEnum terminationState, string jsonResult);
        
    }

}
