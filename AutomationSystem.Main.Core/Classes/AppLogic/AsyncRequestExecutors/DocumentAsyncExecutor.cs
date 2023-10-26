using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Certificates.System;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.MainAsyncRequestManagers.System.Models;
using AutomationSystem.Main.Core.Reports.System;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Model;
using Newtonsoft.Json;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using System;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Classes.System.Emails;

namespace AutomationSystem.Main.Core.Classes.AppLogic.AsyncRequestExecutors
{
    /// <summary>
    /// Executes request for generating reports, certificates and other documents
    /// </summary>
    public class DocumentAsyncExecutor : IAsyncRequestExecutor
    {                

        private readonly IReportService reportService;
        private readonly ICertificateService certificateService;
        private readonly IClassDatabaseLayer classDb;
        private readonly IMainFileService mainFileService;
        private readonly IEmailAttachmentProviderFactory emailAttachmentProviderFactory;
        private readonly ITracerFactory tracerFactory;
        private readonly IClassEmailService classEmailService;
        private readonly IClassTypeResolver classTypeResolver;

        private ITracer tracer;

        // can reports incident
        public bool CanReportIncident => true;


        // constructor
        public DocumentAsyncExecutor(
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


        // executes async operation
        public AsyncRequestExecutorResult Execute(AsyncRequest request)
        {
            tracer = tracerFactory.CreateTracer<DocumentAsyncExecutor>(request.AsyncRequestId);

            // checks request
            var parameters = CheckAsyncRequestAndGetRootPath(request);

            // loads class and makes initial checking
            var classId = request.EntityId ?? 0;
            var cls = classDb.GetClassById(classId);
            CheckClassForAsyncOperation(classId, cls, request.AsyncRequestTypeId);

            // execute operations
            try
            {
                tracer.Info($"Processing of asyncRequestType = {request.AsyncRequestTypeId}");
                switch (request.AsyncRequestTypeId)
                {
                    // generates and sends final reports
                    case AsyncRequestTypeEnum.SendFinalReports:

                        // generates reports                       
                        var classFileIds = reportService.GenerateClassReportsForFinalReports(parameters.ReportRootPath, classId);

                        // sends final report emails
                        var attachmentFileIds = mainFileService.GetFileIdsByClassFileIds(classFileIds);
                        var attachments = emailAttachmentProviderFactory.CreateSimpleEmailAttachmentProvider(attachmentFileIds.ToArray());
                        classEmailService.SendClassTextMapEmailByTypeToRecipient(EmailTypeEnum.ClassFinalReports,
                            classId, new Dictionary<string, object>(), RecipientType.Coordinator, attachments);
                        break;                   

                    // generates certificats
                    case AsyncRequestTypeEnum.GenerateCertificates:
                        certificateService.GenerateCertificates(parameters.CertificateRootPath, classId);
                        break;

                    // generates class reports
                    case AsyncRequestTypeEnum.GenerateFinancialForms:                       
                        reportService.GenerateClassReportsForFinalReports(parameters.ReportRootPath, classId);
                        break;
                }
            }
            catch (Exception e)
            {
                tracer.Error(e, "Document operation causes error");
                return new AsyncRequestExecutorResult(e, IncidentTypeEnum.ReportError, EntityTypeEnum.MainClass, classId);
            }
            return new AsyncRequestExecutorResult();
        }

        #region Checkers

        // checks async request - gets document request parameters
        private DocumentRequestParameters CheckAsyncRequestAndGetRootPath(AsyncRequest request)
        {
            if (request.EntityTypeId != EntityTypeEnum.MainClass || !request.EntityId.HasValue)
                throw new ArgumentException($"Async request has unsupported entity type {request.EntityTypeId} or entity id is null.");

            if (request.JsonParameter == null)
                throw new ArgumentException($"Json parameter is null.");
          
            var result = JsonConvert.DeserializeObject<DocumentRequestParameters>(request.JsonParameter);
            if (result == null)
                throw new ArgumentException($"Document request parameters are null.");
            return result;
        }


        // checks class for async operation
        private void CheckClassForAsyncOperation(long classId, Class cls, AsyncRequestTypeEnum asyncRequestTypeId)
        {
            if (cls == null)
                throw new ArgumentException($"There is no Class with id {classId}.");

            // executes checks for each document type
            switch (asyncRequestTypeId)
            {

                case AsyncRequestTypeEnum.SendFinalReports:
                    if (!classTypeResolver.AreReportsAllowed(cls.ClassCategoryId))
                        throw new InvalidOperationException($"Sending of final reports is not allowed for class category {cls.ClassCategoryId}.");
                    break;              

                case AsyncRequestTypeEnum.GenerateCertificates:
                    if (!classTypeResolver.AreCertificatesAllowed(cls.ClassCategoryId))
                        throw new InvalidOperationException($"Generating of certificate is not allowed for class category {cls.ClassCategoryId}.");                                        
                    break;

                case AsyncRequestTypeEnum.GenerateFinancialForms:
                    if (!classTypeResolver.AreFinancialFormsAllowed(cls.ClassCategoryId))
                        throw new InvalidOperationException($"Generating of final reports is not allowed for class category {cls.ClassCategoryId}.");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(asyncRequestTypeId), asyncRequestTypeId, 
                        $"DocumentAsyncExecutor cannot process operation {asyncRequestTypeId}.");
            }
        }       

        #endregion

    }

}
