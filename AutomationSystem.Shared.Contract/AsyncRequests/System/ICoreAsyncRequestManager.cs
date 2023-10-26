using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.AsyncRequests.System
{
    /// <summary>
    /// Manages manual running of asynchronous task and jobs
    /// </summary>
    public interface ICoreAsyncRequestManager
    {

        // gets last request by entity and type
        AsyncRequestDetail GetLastRequestByEntityAndType(EntityTypeEnum? entityTypeId, long? entityId, AsyncRequestTypeEnum type);

        // gets last requests by entity and types
        List<AsyncRequestDetail> GetLastRequestsByEntityAndTypes(EntityTypeEnum? entityTypeId, long? entityId, params AsyncRequestTypeEnum[] types);


        // adds request for incident
        AsyncRequest AddReportIncidentRequest(long incidentId, int severity);

        // adds request for email sending
        AsyncRequest AddSendEmailRequest(long emailId, int severity);

        // adds async request
        AsyncRequest AddAsyncRequest(EntityTypeEnum entityTypeId, long entityId, AsyncRequestTypeEnum asyncRequestTypeId, int severity, string jsonParameter = null);

        // adds async request with parameters
        AsyncRequest AddAsyncRequestWithParameters<T>(EntityTypeEnum entityTypeId, long entityId, AsyncRequestTypeEnum asyncRequestTypeId, T parameter, int severity);

    }
}
