using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Contract.Preferences.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Emails.System.AsyncRequestExecutors
{
    /// <summary>
    /// Factory for EmailSenderExecutor
    /// </summary>
    public class EmailSenderExecutorFactory : IAsyncRequestExecutorFactory
    {
        public readonly ICorePreferenceProvider preferences;
        public readonly IEmailDatabaseLayer emailDb;
        public readonly ICoreFileService fileService;
        public readonly ITracerFactory tracerFactory;

        public HashSet<AsyncRequestTypeEnum> SupportedAsyncRequestTypes => new HashSet<AsyncRequestTypeEnum> { AsyncRequestTypeEnum.SendEmail };

        public EmailSenderExecutorFactory(
            ICorePreferenceProvider preferences,
            IEmailDatabaseLayer emailDb,
            ICoreFileService fileService,
            ITracerFactory tracerFactory)
        {
            this.preferences = preferences;
            this.emailDb = emailDb;
            this.fileService = fileService;
            this.tracerFactory = tracerFactory;
        }

        public IAsyncRequestExecutor CreateAsyncRequestExecutor()
        {
            return new EmailSenderExecutor(preferences, emailDb, fileService, tracerFactory);
        }
    }
}
