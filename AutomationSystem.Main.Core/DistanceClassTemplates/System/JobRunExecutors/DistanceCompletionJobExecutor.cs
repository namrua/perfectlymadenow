using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Certificates;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.System.Models;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Contract.Jobs.System.Models;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.System.JobRunExecutors
{
    public class DistanceCompletionJobExecutor : IJobRunExecutor
    {
        private readonly IDistanceClassTemplateService distanceTemplateService;
        private readonly IDistanceClassTemplateDatabaseLayer distanceTemplateDb;
        private readonly ICoreEmailService coreEmailService;
        private readonly ITracerFactory tracerFactory;

        private ITracer tracer;
        private List<IncidentForLog> incidents;

        public DistanceCompletionJobExecutor(
            IDistanceClassTemplateService distanceTemplateService,
            IDistanceClassTemplateDatabaseLayer distanceTemplateDb,
            ICoreEmailService coreEmailService,
            ITracerFactory tracerFactory)
        {
            this.distanceTemplateService = distanceTemplateService;
            this.distanceTemplateDb = distanceTemplateDb;
            this.coreEmailService = coreEmailService;
            this.tracerFactory = tracerFactory;
        }

        public bool CanReportIncident => true;

        public JobRunExecutorResult Execute(JobRun jobRun)
        {
            tracer = tracerFactory.CreateTracer<DistanceCompletionJobExecutor>(jobRun.JobRunId);
            incidents = new List<IncidentForLog>();
            var certificateRootPath = Path.Combine(ConfigurationManager.AppSettings["WebRootPath"], CertificateConstants.CertificateRootPathForJob);

            // loads templates for completion
            var filter = new DistanceClassTemplateFilter
            {
                TemplateState = DistanceClassTemplateState.Approved,
                ToAutomationCompleteTime = DateTime.UtcNow
            };
            var templatesToComplete = distanceTemplateDb.GetDistanceClassTemplatesByFilter(filter);
            tracer.Info($"Distance class templates to complete loaded: [{string.Join(", ", templatesToComplete.Select(x => x.DistanceClassTemplateId))}].");

            // completes templates
            var completionResults = new List<DistanceClassTemplateCompletionResult>();
            foreach (var templateToComplete in templatesToComplete)
            {
                var distanceTemplateId = templateToComplete.DistanceClassTemplateId;
                try
                {
                    // complete class
                    tracer.Info($"Processing of distance class template {distanceTemplateId}.");
                    var completionResult = distanceTemplateService.CompleteDistanceClassTemplate(distanceTemplateId, certificateRootPath);

                    // process result
                    completionResults.Add(completionResult);
                    if (completionResult.IsSuccess)
                    {
                        tracer.Info($"Distance class template {distanceTemplateId} completed.");
                        continue;
                    }

                    // process error
                    tracer.Error(completionResult.Exception, $"Processing of distance class template {distanceTemplateId} causes error. CorruptedClassId: {completionResult.CorruptedClassId}.");
                    incidents.Add(CreateIncidentForLogByCompletionResult(completionResult));
                }
                catch (Exception e)
                {
                    tracer.Error(e, $"Processing of distance class template {distanceTemplateId} causes error.");
                    var incident = IncidentForLog
                        .New(IncidentTypeEnum.JobRunError, e)
                        .Entity(EntityTypeEnum.MainDistanceClassTemplate, distanceTemplateId);
                    incidents.Add(incident);
                }
            }

            // send notification to administrator
            SendReportEmail(completionResults, jobRun.JobRunId);

            // process incidents to result
            var result = new JobRunExecutorResult();
            if (incidents.Count > 0)
            {
                result.IsSuccess = false;
                result.Incident = IncidentForLog
                    .New(IncidentTypeEnum.JobRunError, "DistanceCompletionJobExecutor encounters problems", "See inner incidents...")
                    .Entity(EntityTypeEnum.CoreJobRun, jobRun.JobRunId)
                    .AddChildrenCollection(incidents);
            }
            return result;
        }

        #region private methods

        private IncidentForLog CreateIncidentForLogByCompletionResult(DistanceClassTemplateCompletionResult completionResult)
        {
            var incident = IncidentForLog.New(IncidentTypeEnum.JobRunError, completionResult.Exception);
            incident = completionResult.CorruptedClassId.HasValue 
                ? incident.Entity(EntityTypeEnum.MainClass, completionResult.CorruptedClassId) 
                : incident.Entity(EntityTypeEnum.MainDistanceClassTemplate, completionResult.DistanceClassTemplateId);
            return incident;
        }

        private void SendReportEmail(List<DistanceClassTemplateCompletionResult> completionResults, long jobRunId)
        {
            if (!completionResults.Any())
            {
                return;
            }

            try
            {
                var textMap = new Dictionary<string, object>
                {
                    ["DistanceClassCompletionJobReport"] = CreateReportContent(completionResults)
                };
                coreEmailService.SendJobReportEmail(EmailTypeEnum.DistanceClassCompletionJobReport, (int)SeverityEnum.High, jobRunId, textMap);
                tracer.Info("Report email was processed.");
            }
            catch (Exception e)
            {
                tracer.Error(e, "Report email causes error.");
                var incident = IncidentForLog
                    .New(IncidentTypeEnum.JobRunError, e)
                    .Entity(EntityTypeEnum.CoreJobRun, jobRunId);
                incidents.Add(incident);
            }
        }

        private string CreateReportContent(List<DistanceClassTemplateCompletionResult> completionResults)
        {
            var sb = new StringBuilder();
            foreach (var completionResult in completionResults)
            {
                if (completionResult.IsSuccess)
                {
                    sb.AppendLine($"Distance class template with id {completionResult.DistanceClassTemplateId} completed. ");
                }
                else
                {
                    sb.AppendLine($"Completion of distance class template with id {completionResult.DistanceClassTemplateId} fails.");
                }

                if (completionResult.CompletedClasses.Any())
                {
                    sb.AppendLine($"* Completed classes: [{string.Join(", ", completionResult.CompletedClasses)}].");
                }

                if (completionResult.SkippedClasses.Any())
                {
                    sb.AppendLine($"* Skipped classes: [{string.Join(", ", completionResult.SkippedClasses)}].");
                }

                if (!completionResult.IsSuccess)
                {
                    sb.AppendLine($"* Corrupted class: {completionResult.CorruptedClassId}.\n{completionResult.Exception}");
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
