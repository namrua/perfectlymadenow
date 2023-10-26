using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Core.AsyncRequests.Data.Extensions;
using AutomationSystem.Shared.Core.AsyncRequests.Data.Models;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.AsyncRequests.Data
{
    /// <summary>
    /// Asynchronous Request and Jobs database layer
    /// </summary>
    public class AsyncDatabaseLayer : IAsyncDatabaseLayer
    {

        // gets async request by entity and type
        public List<AsyncRequest> GetAsyncRequestsByEntityAndType(EntityTypeEnum? entityTypeId, long? entityId, 
            IEnumerable<AsyncRequestTypeEnum> types, AsyncRequestIncludes includes = AsyncRequestIncludes.None)
        {
            using (var context = new CoreEntities())
            {
                var result = context.AsyncRequests.AddIncludes(includes)
                    .Where(x => x.EntityTypeId == entityTypeId && x.EntityId == entityId && types.Contains(x.AsyncRequestTypeId)).ToList();
                return result;
            }
        }


        // inserts new async request
        public AsyncRequest InsertAsyncRequest(AsyncRequestTypeEnum type, EntityTypeEnum? entityType, long? entityId, int severity, string jsonParameter = null)
        {            
            using (var context = new CoreEntities())
            {                             
                // adds async request
                var request = new AsyncRequest();
                request.AsyncRequestTypeId = type;
                request.EntityTypeId = entityType;
                request.EntityId = entityId;
                request.JsonParameter = jsonParameter;
                request.ProcessingStateId = ProcessingStateEnum.New;
                request.Severity = severity;
                request.Active = true;
                request.Planned = DateTime.Now;
                context.AsyncRequests.Add(request);
                context.SaveChanges();
                return request;
            }
        }


        // loads async request and set it to InProcess when it is active and new
        public AsyncRequest GetAsyncRequestToProcessing(long asyncRequestId, out bool wasStarted)
        {
            using (var context = new CoreEntities())
            {   
                // loads result
                wasStarted = false;
                var result = context.AsyncRequests.Active().FirstOrDefault(x => x.AsyncRequestId == asyncRequestId);

                // if result is new set as processed
                if (result != null && result.Active && result.ProcessingStateId == ProcessingStateEnum.New)
                {
                    wasStarted = true;
                    result.ProcessingStateId = ProcessingStateEnum.InProcess;
                    result.Started = DateTime.Now;
                    context.SaveChanges();
                }
                return result;
            }
        }

        // sets async request state
        public void FinishAsyncRequest(long asyncRequestId, ProcessingStateEnum terminationState, string jsonResult)
        {
            using (var context = new CoreEntities())
            {
                var result = context.AsyncRequests.Active().First(x => x.AsyncRequestId == asyncRequestId);
                result.ProcessingStateId = terminationState;
                result.Finished = DateTime.Now;
                result.JsonResult = jsonResult;        
                context.SaveChanges();                
            }
        }

    }

}
