using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.DistanceClassTemplates.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.AppLogic.AsyncRequestExecutors
{
    public class CompleteDistanceClassesForTemplateAsyncRequestExecutorFactory : IAsyncRequestExecutorFactory
    {
        private readonly IDistanceClassTemplateService templateService;
        private readonly ITracerFactory tracerFactory;

        public CompleteDistanceClassesForTemplateAsyncRequestExecutorFactory(
            IDistanceClassTemplateService templateService,
            ITracerFactory tracerFactory)
        {
            this.templateService = templateService;
            this.tracerFactory = tracerFactory;
        }

        public HashSet<AsyncRequestTypeEnum> SupportedAsyncRequestTypes => new HashSet<AsyncRequestTypeEnum>
        {
            AsyncRequestTypeEnum.CompleteDistanceClassesForTemplate
        };

        public IAsyncRequestExecutor CreateAsyncRequestExecutor()
        {
            var result = new CompleteDistanceClassesForTemplateAsyncRequestExecutor(templateService, tracerFactory);
            return result;
        }
    }
}
