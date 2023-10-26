using System;
using System.Collections.Generic;
using System.Web;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Certificates;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.AsyncRequestExecutors;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Main.Core.MainAsyncRequestManagers.System
{
    /// <summary>
    /// Manages async request for main entities
    /// </summary>
    public class MainAsyncRequestManager : IMainAsyncRequestManager
    {
        private readonly ICoreAsyncRequestManager coreAsyncManager;
        private readonly IRegistrationTypeResolver registrationTypeResolver;

        // constructor
        public MainAsyncRequestManager(ICoreAsyncRequestManager coreAsyncManager, IRegistrationTypeResolver registrationTypeResolver)
        {
            this.coreAsyncManager = coreAsyncManager;
            this.registrationTypeResolver = registrationTypeResolver;
        }


        // gets last request by entity and type
        public AsyncRequestDetail GetLastRequestByEntityAndType(EntityTypeEnum? entityTypeId, long? entityId, AsyncRequestTypeEnum type)
        {
            var result = coreAsyncManager.GetLastRequestByEntityAndType(entityTypeId, entityId, type);
            return result;
        }

        // gets last requests by entity and types
        public List<AsyncRequestDetail> GetLastRequestsByEntityAndTypes(EntityTypeEnum? entityTypeId, long? entityId, params AsyncRequestTypeEnum[] types)
        {
            var result = coreAsyncManager.GetLastRequestsByEntityAndTypes(entityTypeId, entityId, types);
            return result;
        }


        // adds request for adding registration to WebEx
        public AsyncRequest AddIntegrationRequestForClassRegistration(ClassRegistration registration, AsyncRequestTypeEnum integrationRequestTypeId, int severity)
        {
            if (registrationTypeResolver.IsWwaRegistration(registration.RegistrationTypeId))
            {
                return null;
            }

            var result = coreAsyncManager.AddAsyncRequest(EntityTypeEnum.MainClassRegistration, registration.ClassRegistrationId, integrationRequestTypeId, severity);
            return result;
        }


        // add document generating request for class
        public AsyncRequest AddDocumentRequestForClass(long classId, AsyncRequestTypeEnum documentRequestTypeId, int severity)
        {
            if (HttpContext.Current == null || HttpContext.Current.Server == null)
            {
                throw new InvalidOperationException("Cannot access rootPath from HttpContext.Current.Server.");
            }

            // assembles json parameters
            var parameters = new DocumentRequestParameters
            {
                ReportRootPath = HttpContext.Current.Server.MapPath(ReportConstants.ReportRootPath),
                CertificateRootPath = HttpContext.Current.Server.MapPath(CertificateConstants.CertificateRootPath)
            };
            var result = coreAsyncManager.AddAsyncRequestWithParameters(EntityTypeEnum.MainClass, classId, documentRequestTypeId, parameters, severity);
            return result;
        }

        // add request by distanceTemplateId
        public AsyncRequest AddCompletionDistanceClassForTemplateAsyncRequest(long distanceTemplateId, int severity)
        {
            if (HttpContext.Current == null || HttpContext.Current.Server == null)
            {
                throw new InvalidOperationException("Cannot access rootPath from HttpContext.Current.Server.");
            }

            var parameters = new CompleteDistanceClassesForTemplateAsyncRequestParams
            {
                CertificateRootPath = HttpContext.Current.Server.MapPath(CertificateConstants.CertificateRootPath)
            };

            var result = coreAsyncManager.AddAsyncRequestWithParameters(
                EntityTypeEnum.MainDistanceClassTemplate,
                distanceTemplateId,
                AsyncRequestTypeEnum.CompleteDistanceClassesForTemplate,
                parameters,
                severity);
            return result;
        }
    }
}
