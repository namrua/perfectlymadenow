using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Contract.Emails.Data;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Contract.Preferences.System;
using AutomationSystem.Shared.Core.Emails.System.AsyncRequestExecutors;
using AutomationSystem.Shared.Core.Incidents.Data;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Incidents.System.AsyncRequestExecutors
{
    /// <summary>
    /// Executes reporting of incidents via emails
    /// </summary>
    public class EmailIncidentReportingExecutor : IAsyncRequestExecutor
    {
        private readonly IEmailSendingDataWrapper emailWrapper;             
        private readonly IIncidentDatabaseLayer incidentDb;
        private readonly ICoreEmailService coreEmailService;
        private readonly ITracerFactory tracerFactory;

        private ITracer tracer;

        public EmailIncidentReportingExecutor(
            ICorePreferenceProvider preferences,
            IEmailDatabaseLayer emailDb,
            IIncidentDatabaseLayer incidentDb,
            ICoreEmailService coreEmailService,
            ICoreFileService fileService,
            ITracerFactory tracerFactory)
        {          
            this.incidentDb = incidentDb;
            this.coreEmailService = coreEmailService;
            this.tracerFactory = tracerFactory;
            
            emailWrapper = new EmailSendingDataWrapper(preferences, emailDb, fileService, tracerFactory);       
        }

        public bool CanReportIncident => false;

        public AsyncRequestExecutorResult Execute(AsyncRequest request)
        {
            tracer = tracerFactory.CreateTracer<EmailIncidentReportingExecutor>(request.AsyncRequestId);
            if (request.EntityTypeId != EntityTypeEnum.CoreIncident || !request.EntityId.HasValue)
                throw new ArgumentException($"Async request has unsupported entity type {request.EntityTypeId} or entity id is null.");            

            // loads incident
            var incident = GetIncident(request.EntityId.Value);
            tracer.Info($"Incident with id {incident.IncidentId} was loaded");
            try
            {
                // creates email in db
                var emailId = coreEmailService.SendIncidentEmail(EmailTypeEnum.CoreIncidentWarning, (int)SeverityEnum.Low, incident);
                tracer.Info($"Incident warning email with id {emailId} was saved");

                // cretes email message for sender
                var message = emailWrapper.GetEmailMessageByEmailId(emailId);
                tracer.Info("Email message was created");

                // sends message
                emailWrapper.SendEmailMessage(message);
                tracer.Info("Email sending was successfully finished");

                // set incident as reported
                incidentDb.UpdateIncidentReportingState(incident.IncidentId, true, DateTime.Now, incident.ReportingAttempts + 1);
                tracer.Info($"Incident {incident.IncidentId} was set as reported");
                return new AsyncRequestExecutorResult();
            }
            catch (Exception e)
            {
                incidentDb.UpdateIncidentReportingState(incident.IncidentId, false, null, incident.ReportingAttempts + 1);
                tracer.Error(e, "Incident reporting caused error, reporting attempts was updated");
                return new AsyncRequestExecutorResult(e, IncidentTypeEnum.EmailError, EntityTypeEnum.CoreIncident, incident.IncidentId);
            }
        }

        #region private methods

        private Incident GetIncident(long incidentId)
        {
            var result = incidentDb.GetIncidentById(incidentId);
            if (result == null)
                throw new ArgumentException($"There is no incident with id {incidentId}");
            if (result.IsReported)
                throw new InvalidOperationException($"Incident with id {incidentId} is already reported");
            if (!result.CanBeReport)
                throw new InvalidOperationException($"Incident with id {incidentId} cannot be reported");
            return result;
        }

        #endregion

    }

}
