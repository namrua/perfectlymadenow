using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Core.AsyncRequests.Data;
using AutomationSystem.Shared.Core.AsyncRequests.Data.Models;
using AutomationSystem.Shared.Model;
using Newtonsoft.Json;

namespace AutomationSystem.Shared.Core.AsyncRequests.System
{
    /// <summary>
    /// Manages manual running of asynchronous task and jobs
    /// </summary>
    public class CoreAsyncRequestManager : ICoreAsyncRequestManager
    {

        // private components
        private readonly IAsyncRequestExecutorManagerFactory asyncManagerFactory;
        private readonly IAsyncDatabaseLayer asyncDb;
        private readonly IAsyncRequestConvertor asyncConvertor;

        // constructor
        public CoreAsyncRequestManager(IAsyncDatabaseLayer asyncDb, IAsyncRequestExecutorManagerFactory asyncManagerFactory)
        {
            this.asyncDb = asyncDb;
            this.asyncManagerFactory = asyncManagerFactory;
            asyncConvertor = new AsyncRequestConvertor();
        }


        // gets last request by entity and type
        public AsyncRequestDetail GetLastRequestByEntityAndType(EntityTypeEnum? entityTypeId, long? entityId, AsyncRequestTypeEnum type)
        {            
            var requests = asyncDb.GetAsyncRequestsByEntityAndType(entityTypeId, entityId, new[] {type},
                AsyncRequestIncludes.ProcessingState | AsyncRequestIncludes.AsyncRequestType);
            var lastRequest = SelectRelevantAsyncRequest(requests, DateTime.Now);
            var result = asyncConvertor.ConvertToAsyncRequestDetail(lastRequest);
            return result;
        }

        // gets last requests by entity and types
        public List<AsyncRequestDetail> GetLastRequestsByEntityAndTypes(EntityTypeEnum? entityTypeId, long? entityId, params AsyncRequestTypeEnum[] types)
        {
            var now = DateTime.Now;
            var requests = asyncDb.GetAsyncRequestsByEntityAndType(entityTypeId, entityId, types,
                AsyncRequestIncludes.ProcessingState | AsyncRequestIncludes.AsyncRequestType);
            var result = requests.GroupBy(x => x.AsyncRequestTypeId)
                .Select(x => SelectRelevantAsyncRequest(x, now))
                .Select(asyncConvertor.ConvertToAsyncRequestDetail).ToList();            
            return result;
        }


        // adds request for incident
        public AsyncRequest AddReportIncidentRequest(long incidentId, int severity)
        {
            var result = asyncDb.InsertAsyncRequest(AsyncRequestTypeEnum.ReportIncident,
                EntityTypeEnum.CoreIncident, incidentId, severity);
            Execute(result);
            return result;
        }

        // adds request for email sending
        public AsyncRequest AddSendEmailRequest(long emailId, int severity)
        {
            var result = asyncDb.InsertAsyncRequest(AsyncRequestTypeEnum.SendEmail,
                EntityTypeEnum.CoreEmail, emailId, severity);
            Execute(result);
            return result;
        }


        // adds async request
        public AsyncRequest AddAsyncRequest(EntityTypeEnum entityTypeId, long entityId, AsyncRequestTypeEnum asyncRequestTypeId, int severity, string jsonParameter = null)
        {           
            var result = asyncDb.InsertAsyncRequest(asyncRequestTypeId, entityTypeId, entityId, severity, jsonParameter);
            Execute(result);
            return result;
        }
        
        // adds async request with parameters
        public AsyncRequest AddAsyncRequestWithParameters<T>(EntityTypeEnum entityTypeId, long entityId, AsyncRequestTypeEnum asyncRequestTypeId, T parameter, int severity)
        {
            var jsonParameter = JsonConvert.SerializeObject(parameter);
            var result = AddAsyncRequest(entityTypeId, entityId, asyncRequestTypeId, severity, jsonParameter);
            return result;
        }


        #region private fields

        // executes request
        private void Execute(AsyncRequest request)
        {
            // todo: returning of current asyncrequest for sync processing, throwing of exceptions?
            if (request.Severity <= (int) SeverityEnum.Fatal)
            {
                asyncManagerFactory.CreateAsyncRequestExecutorManager().Execute(request.AsyncRequestId);
                return;
            }

            if (request.Severity <= (int) SeverityEnum.High)
            {
                asyncManagerFactory.CreateAsyncRequestExecutorManager().ExecuteAsync(request.AsyncRequestId);
            }
        }

        // selects last relevant AsyncRequest 
        private AsyncRequest SelectRelevantAsyncRequest(IEnumerable<AsyncRequest> requests, DateTime now)
        {
            // filters request planned in the future, take the newest one
            var result = requests.OrderByDescending(x => x.Planned).FirstOrDefault(x => x.Planned <= now);
            return result;
        }

        #endregion
    }

}
