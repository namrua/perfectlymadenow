using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.Integration.System.Models;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.WebEx.Contract.Connectors;
using PerfectlyMadeInc.WebEx.Contract.Connectors.Models;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace AutomationSystem.Main.Core.Integration.System.WebEx
{
    /// <summary>
    /// Executes registration synchronization to WebEx
    /// </summary>
    public class WebExRegistrationIntegrationSyncExecutor : IRegistrationIntegrationSyncExecutor
    {

        private readonly IIntegrationSyncExecutor syncExecutor;
        private readonly IWebExRegistrationConvertor registrationConvertor;
        private readonly ITracerFactory tracerFactory;

        private StringBuilder reportContent;

        public WebExRegistrationIntegrationSyncExecutor(IIntegrationSyncExecutor syncExecutor, ITracerFactory tracerFactory, IWebExRegistrationConvertor registrationConvertor)
        {
            this.syncExecutor = syncExecutor;
            this.tracerFactory = tracerFactory;
            this.registrationConvertor = registrationConvertor;
        }

        public RegistrationIntegrationSyncResult ExecuteSync(JobRun jobRun, Class cls, List<ClassRegistration> approvedClosedRegistrations)
        {
            var tracer = tracerFactory.CreateTracer<WebExRegistrationIntegrationSyncExecutor>(jobRun.JobRunId);

            reportContent = new StringBuilder();
            if (cls.IntegrationTypeId != IntegrationTypeEnum.WebExProgram || !cls.IntegrationEntityId.HasValue)
                throw new ArgumentException($"There is invalid integration type {cls.IntegrationTypeId} or null integration entity id.");

            // executes synchronization    
            tracer.Info("Starts integration sync executor.");
            var systemStates = approvedClosedRegistrations.Select(x => registrationConvertor.ConvertToIntegrationState(x)).ToList();
            syncExecutor.Initialize(systemStates);
            var syncResult = syncExecutor.ExecuteSyncForProgram(cls.IntegrationEntityId.Value);
            tracer.Info("Integration sync executor finished.");

            // assembles result
            var result = new RegistrationIntegrationSyncResult();
            if (syncResult.WasError)
                result.Incidents = ExtractProgramErrors(cls, syncResult);
            WriteProgramToReportContent(cls, syncResult);
            result.ReportContent = reportContent.ToString();
            return result;
        }
        
        #region incident extraction

        private List<IncidentForLog> ExtractProgramErrors(Class cls, ProgramSyncResult programSyncResult)
        {
            var result = new List<IncidentForLog>();

            // process program error
            if (programSyncResult.Exception != null)
            {
                var programIncident = IncidentForLog
                    .New(IncidentTypeEnum.OuterSystemError, programSyncResult.Exception)
                    .Entity(EntityTypeEnum.MainClass, cls.ClassId);
                result.Add(programIncident);
            }

            // process events errors
            result.AddRange(programSyncResult.EventResults.Where(x => x.WasError).SelectMany(x => ExtractEventErrors(cls, x)));

            return result;
        }

        private List<IncidentForLog> ExtractEventErrors(Class cls, EventSyncResult eventSyncResult)
        {
            var result = new List<IncidentForLog>();

            // process event error
            if (eventSyncResult.Exception != null)
            {
                var eventIncident = IncidentForLog
                    .New(IncidentTypeEnum.OuterSystemError, eventSyncResult.Exception)
                    .Entity(EntityTypeEnum.MainClass, cls.ClassId);
                result.Add(eventIncident);
            }

            // process action errors
            foreach (var syncActionResult in eventSyncResult.SyncActions.Where(x => x.WasError))
            {
                var systemState = syncActionResult.ConsistencyResult.SystemState;
                var actionIncident = IncidentForLog
                    .New(IncidentTypeEnum.OuterSystemError, syncActionResult.Exception)
                    .Entity(systemState.EntityTypeId, systemState.EntityId);
                result.Add(actionIncident);
            }

            return result;
        }

        #endregion

        #region report creation

        private void WriteProgramToReportContent(Class cls, ProgramSyncResult syncResult)
        {            
            reportContent.AppendLine($"Class '{ClassConvertor.GetClassTitle(cls)}' - Program '{syncResult.ProgramName}'");
            WriteErrorToReportContent(syncResult.Exception);           
            foreach (var syncEvent in syncResult.EventResults)
            {
                reportContent.AppendLine();
                WriteEventToReportContent(syncEvent);                
            }
        }

        private void WriteEventToReportContent(EventSyncResult syncEvent)
        {   
            // tests whether there is no issue to be reported            
            if (!syncEvent.HasIssue)
            {
                reportContent.AppendLine($"* Event '{syncEvent.EventName}' - everything is correct.");
                return;
            }

            // otherwise writes all issues
            reportContent.AppendLine($"* Event '{syncEvent.EventName}'");
            WriteErrorToReportContent(syncEvent.Exception);
           
            WriteDuplicitEmailsToReportContent(syncEvent.DuplicitEmails);
            WriteSyncActionToReportContent(syncEvent.SyncActions);
            WriteUnknownInWebExToReportContent(syncEvent.UnknownInWebEx);
            WriteInconsistenciesToReportContent(syncEvent.Inconsistent);
            WriteErrorStatesToReportContent(syncEvent.ErrorStates);
        }       

        private void WriteDuplicitEmailsToReportContent(Dictionary<string, List<IntegrationStateDto>> duplicitEmails)
        {
            if (!duplicitEmails.Any()) return;
            reportContent.AppendLine();
            reportContent.AppendLine("Duplicit emails of students:");
            foreach (var pair in duplicitEmails)
                reportContent.AppendLine($"- {pair.Key} - {string.Join(", ", pair.Value.Select(GetName))}");
        }

        private void WriteSyncActionToReportContent(List<SyncActionResult> syncAction)
        {
            if (!syncAction.Any()) return;
            reportContent.AppendLine();
            reportContent.AppendLine("Synchronization Actions:");
            foreach (var action in syncAction)
            {
                reportContent.Append($"- {GetName(action.ConsistencyResult.SystemState)} ");
                switch (action.ConsistencyResult.OperationType)
                {                       
                    case SyncOperationType.Save:
                        reportContent.AppendLine("was added or updated in the WebEx.");
                        break;
                    case SyncOperationType.Remove:
                        reportContent.AppendLine("was removed from the WebEx.");
                        break;                      
                }
                WriteErrorToReportContent(action.Exception, "  ");
            }
        }

        private void WriteUnknownInWebExToReportContent(List<IntegrationStateDto> unknownInWebEx)
        {
            if (!unknownInWebEx.Any()) return;
            reportContent.AppendLine();
            reportContent.AppendLine("Following students in the WebEx are unknown to the System:");
            foreach(var unknown in unknownInWebEx)
                reportContent.AppendLine($"- {GetName(unknown)}");
        }

        private void WriteInconsistenciesToReportContent(List<ConsistencyResult> inconsistencies)
        {
            if (!inconsistencies.Any()) return;
            reportContent.AppendLine();
            reportContent.AppendLine("Following students remain inconsistent:");
            foreach (var inconsistency in inconsistencies)
            {
                reportContent.AppendLine($"- {GetName(inconsistency.SystemState)}");
                reportContent.AppendLine($"  Error message: {WebExTextHelper.GetInconsistencyTypeText(inconsistency.InconsistencyType)}");
            }
        }

        private void WriteErrorStatesToReportContent(List<IntegrationStateDto> errorStates)
        {
            if (!errorStates.Any()) return;
            reportContent.AppendLine();
            reportContent.AppendLine("Following registrations encountered integration error:");
            foreach (var errorState in errorStates)
            {
                reportContent.AppendLine($"- {errorState.EntityTypeId}({errorState.EntityId})");
                reportContent.AppendLine($"  Error message: {errorState.ErrorMessage}");
            }
        }

        private void WriteErrorToReportContent(Exception e, string prefix = "")
        {
            if (e == null) return;
            reportContent.AppendLine($"{prefix}Error message: {e.Message}");
        }

        private string GetName(IntegrationStateDto state)
        {
            return $"{state.FirstName} {state.LastName} ({state.Email})";
        }

        #endregion
    }
}
