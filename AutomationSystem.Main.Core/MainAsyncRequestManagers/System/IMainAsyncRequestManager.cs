using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.MainAsyncRequestManagers.System
{


    /// <summary>
    /// Manages async request for main entities
    /// </summary>
    public interface IMainAsyncRequestManager
    {

        // gets last request by entity and type
        AsyncRequestDetail GetLastRequestByEntityAndType(EntityTypeEnum? entityTypeId, long? entityId, AsyncRequestTypeEnum type);

        // gets last requests by entity and types
        List<AsyncRequestDetail> GetLastRequestsByEntityAndTypes(EntityTypeEnum? entityTypeId, long? entityId, params AsyncRequestTypeEnum[] types);


        // adds registration integration request
        AsyncRequest AddIntegrationRequestForClassRegistration(ClassRegistration registration, AsyncRequestTypeEnum integrationRequestTypeId, int severity);

        // add document generating request for class
        AsyncRequest AddDocumentRequestForClass(long classId, AsyncRequestTypeEnum documentRequestTypeId, int severity);

        // add request by distanceTemplateId
        AsyncRequest AddCompletionDistanceClassForTemplateAsyncRequest(long distanceTemplateId, int severity);

    }

}
