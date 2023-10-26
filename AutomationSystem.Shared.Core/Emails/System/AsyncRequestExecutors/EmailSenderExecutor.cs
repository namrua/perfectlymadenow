using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Contract.Preferences.System;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Emails.System.AsyncRequestExecutors
{
    /// <summary>
    /// Executes sending of emails
    /// </summary>
    public class EmailSenderExecutor : IAsyncRequestExecutor
    {

        // private components
        private readonly IEmailSendingDataWrapper emailWrapper;
        private readonly ITracerFactory tracerFactory;

        private ITracer tracer;

        // constructor
        public EmailSenderExecutor(
            ICorePreferenceProvider preferences,
            IEmailDatabaseLayer emailDb,
            ICoreFileService fileService,
            ITracerFactory tracerFactory)
        {
            this.tracerFactory = tracerFactory;

            emailWrapper = new EmailSendingDataWrapper(preferences, emailDb, fileService, tracerFactory);            
        }


        // can reports incident
        public bool CanReportIncident => true;


        // executes task, returns json result
        public AsyncRequestExecutorResult Execute(AsyncRequest request)
        {
            tracer = tracerFactory.CreateTracer<EmailSenderExecutor>(request.AsyncRequestId);
            if (request.EntityTypeId != EntityTypeEnum.CoreEmail || !request.EntityId.HasValue)
                throw new ArgumentException($"Async request has unsupported entity type {request.EntityTypeId} or entity id is null.");

            // loads email message
            var message = emailWrapper.GetEmailMessageByEmailId(request.EntityId.Value);           
            tracer.Info("Email message was created");
            try
            {
                // sends message
                emailWrapper.SendEmailMessage(message);
                tracer.Info("Email sending was successfully finished");
                return new AsyncRequestExecutorResult();
            }
            catch (Exception e)
            {
                tracer.Error(e, "Email sending causes error");
                return new AsyncRequestExecutorResult(e, IncidentTypeEnum.EmailError, EntityTypeEnum.CoreEmail, message.Email.EmailId);
            }            
        }       

    }

}
