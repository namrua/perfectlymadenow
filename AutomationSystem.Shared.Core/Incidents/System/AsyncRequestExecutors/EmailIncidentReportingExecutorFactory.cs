using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Contract.Preferences.System;
using AutomationSystem.Shared.Core.Incidents.Data;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Incidents.System.AsyncRequestExecutors
{
    /// <summary>
    /// Factory for EmailIncidentReportingExecutor
    /// </summary>
    public class EmailIncidentReportingExecutorFactory : IAsyncRequestExecutorFactory
    {
        private readonly ICorePreferenceProvider preferences;
        private readonly IEmailDatabaseLayer emailDb;
        private readonly IIncidentDatabaseLayer incidentDb;
        private readonly ICoreEmailService coreEmailService;
        private readonly ICoreFileService fileService;
        private readonly ITracerFactory tracerFactory;

        public HashSet<AsyncRequestTypeEnum> SupportedAsyncRequestTypes => new HashSet<AsyncRequestTypeEnum> { AsyncRequestTypeEnum.ReportIncident };

        public EmailIncidentReportingExecutorFactory(
            ICorePreferenceProvider preferences,
            IEmailDatabaseLayer emailDb,
            IIncidentDatabaseLayer incidentDb,
            ICoreEmailService coreEmailService,
            ICoreFileService fileService,
            ITracerFactory tracerFactory)
        {
            this.preferences = preferences;
            this.emailDb = emailDb;
            this.incidentDb = incidentDb;
            this.coreEmailService = coreEmailService;
            this.fileService = fileService;
            this.tracerFactory = tracerFactory;
        }

        public IAsyncRequestExecutor CreateAsyncRequestExecutor()
        {
            return new EmailIncidentReportingExecutor(preferences, emailDb, incidentDb, coreEmailService, fileService, tracerFactory);
        }
    }
}
