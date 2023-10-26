using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Certificates.System;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.Reports.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Emails.System;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Classes.System.Emails;

namespace AutomationSystem.Main.Core.Classes.AppLogic.AsyncRequestExecutors
{
    /// <summary>
    /// Factory for DocumentAsyncExecutor
    /// </summary>
    public class DocumentAsyncExecutorFactory : IAsyncRequestExecutorFactory
    {
        private readonly IReportService reportService;
        private readonly ICertificateService certificateService;
        private readonly IClassDatabaseLayer classDb;
        private readonly IMainFileService mainFileService;
        private readonly IEmailAttachmentProviderFactory emailAttachmentProviderFactory;
        private readonly ITracerFactory tracerFactory;
        private readonly IClassEmailService classEmailService;
        private readonly IClassTypeResolver classTypeResolver;

        public HashSet<AsyncRequestTypeEnum> SupportedAsyncRequestTypes => new HashSet<AsyncRequestTypeEnum>
        {
            AsyncRequestTypeEnum.SendFinalReports,
            AsyncRequestTypeEnum.GenerateCertificates,
            AsyncRequestTypeEnum.GenerateFinancialForms
        };

        public DocumentAsyncExecutorFactory(
            IReportService reportService,
            ICertificateService certificateService,
            IClassDatabaseLayer classDb,
            IMainFileService mainFileService,
            IEmailAttachmentProviderFactory emailAttachmentProviderFactory,
            ITracerFactory tracerFactory,
            IClassEmailService classEmailService,
            IClassTypeResolver classTypeResolver)
        {
            this.reportService = reportService;
            this.certificateService = certificateService;
            this.classDb = classDb;
            this.mainFileService = mainFileService;
            this.emailAttachmentProviderFactory = emailAttachmentProviderFactory;
            this.tracerFactory = tracerFactory;
            this.classEmailService = classEmailService;
            this.classTypeResolver = classTypeResolver;
        }

        public IAsyncRequestExecutor CreateAsyncRequestExecutor()
        {
            return new DocumentAsyncExecutor(
                reportService,
                certificateService,
                classDb,
                mainFileService,
                emailAttachmentProviderFactory,
                tracerFactory,
                classEmailService,
                classTypeResolver);
        }
    }
}
