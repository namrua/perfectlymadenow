using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using System;
using AutomationSystem.Main.Core.DistanceClassTemplates.System.Models;
using Newtonsoft.Json;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.AsyncRequestExecutors
{
    public class CompleteDistanceClassesForTemplateAsyncRequestExecutor : IAsyncRequestExecutor
    {
        private readonly IDistanceClassTemplateService templateService;
        private readonly ITracerFactory tracerFactory;

        private ITracer tracer;

        public CompleteDistanceClassesForTemplateAsyncRequestExecutor(
            IDistanceClassTemplateService templateService,
            ITracerFactory tracerFactory)
        {
            this.templateService = templateService;
            this.tracerFactory = tracerFactory;
        }

        public bool CanReportIncident => true;

        public AsyncRequestExecutorResult Execute(AsyncRequest request)
        {
            tracer = tracerFactory.CreateTracer<CompleteDistanceClassesForTemplateAsyncRequestExecutor>(request.AsyncRequestId);
            if (request.EntityTypeId != EntityTypeEnum.MainDistanceClassTemplate || !request.EntityId.HasValue)
            {
                throw new ArgumentException($"Async request has unsupported entity type {request.EntityTypeId} or entity id is null.");
            }

            var requestParams = GetRequestParams(request);
            DistanceClassTemplateCompletionResult completionResult;
            try
            {
                tracer.Info($"Start completing of distance class template with id: {request.EntityId.Value}.");
                completionResult = templateService.CompleteDistanceClassTemplate(request.EntityId.Value, requestParams.CertificateRootPath);
            }
            catch (Exception e)
            {
                tracer.Error(e, "Distance class template completion causes error.");
                return new AsyncRequestExecutorResult(e, IncidentTypeEnum.AsyncRequestError, EntityTypeEnum.MainDistanceClassTemplate, request.EntityId);
            }

            if (!completionResult.IsSuccess)
            {
                tracer.Error(completionResult.Exception, $"Distance class template failed. Corrupted class id {completionResult.CorruptedClassId}.");
                return new AsyncRequestExecutorResult(completionResult.Exception, IncidentTypeEnum.AsyncRequestError, EntityTypeEnum.MainDistanceClassTemplate, request.EntityId);
            }

            tracer.Info($"Completion of distance class was successful. Skipped classes: [{string.Join(", ", completionResult.SkippedClasses)}]; Completed classes: [{string.Join(", ", completionResult.CompletedClasses)}]."); 
            return new AsyncRequestExecutorResult();
        }

        #region private methods

        private CompleteDistanceClassesForTemplateAsyncRequestParams GetRequestParams(AsyncRequest request)
        {
            if (request.JsonParameter == null)
            {
                throw new ArgumentException("Json parameter is null.");
            }

            var result = JsonConvert.DeserializeObject<CompleteDistanceClassesForTemplateAsyncRequestParams>(request.JsonParameter);
            if (result == null)
            {
                throw new ArgumentException("Request parameters are null.");
            }

            return result;
        }

        #endregion
    }
}
