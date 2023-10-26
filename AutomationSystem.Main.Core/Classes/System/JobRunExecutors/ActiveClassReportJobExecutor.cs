using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Reports.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Emails.System.Models;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.Reports.System;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Contract.Jobs.System.Models;
using AutomationSystem.Shared.Model;
using Newtonsoft.Json;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using AutomationSystem.Main.Core.Classes.System.Emails;

namespace AutomationSystem.Main.Core.Classes.System.JobRunExecutors
{

    /// <summary>
    /// Main report job type
    /// </summary>
    public enum ActiveClassReportJobType
    {
        CoordinatorCrfDailyReport = 1
    }

    /// <summary>
    /// Main report job parameters
    /// </summary>
    public class ActiveClassReportJobParameters
    {
        public ActiveClassReportJobType Type;
    }


    /// <summary>
    /// Executes jobs related with active classes
    /// </summary>
    public class ActiveClassReportJobExecutor : IJobRunExecutor
    {

        private readonly IReportService reportService;
        private readonly IClassDatabaseLayer classDb;
        private readonly IMainFileService mainFileService;
        private readonly IEmailAttachmentProviderFactory emailAttachmentProviderFactory;
        private readonly ITracerFactory tracerFactory;
        private readonly IClassEmailService classEmailService;

        private ITracer tracer;     

        private ActiveClassReportJobParameters parameters;

        // can reports incident
        public bool CanReportIncident => true;


        // constructor
        public ActiveClassReportJobExecutor(
            IReportService reportService,
            IClassDatabaseLayer classDb,
            IMainFileService mainFileService,
            IEmailAttachmentProviderFactory emailAttachmentProviderFactory,
            ITracerFactory tracerFactory,
            IClassEmailService classEmailService)
        {
            this.reportService = reportService;
            this.classDb = classDb;
            this.mainFileService = mainFileService;
            this.emailAttachmentProviderFactory = emailAttachmentProviderFactory;
            this.tracerFactory = tracerFactory;
            this.classEmailService = classEmailService;
        }

        // executes job
        public JobRunExecutorResult Execute(JobRun jobRun)
        {
            tracer = tracerFactory.CreateTracer<ActiveClassReportJobExecutor>(jobRun.JobRunId);

            // gets parameters
            SetJobRunParameters(jobRun.Job.JsonParameter);

            // gets root path for report templates
            var rootPath = Path.Combine(ConfigurationManager.AppSettings["WebRootPath"], ReportConstants.ReportRootPathForJob);

            // loads active classes
            var nowUtc = DateTime.UtcNow;
            var filter = new ClassFilter
            {
                OnlyInAfterRegistration = true,
                EventEndUtcFrom = nowUtc
            };
            var activeClasses = classDb.GetClassesByFilter(filter, ClassIncludes.ClassType);
            tracer.Info($"Active classes was loaded: {string.Join(",", activeClasses.Select(x => x.ClassId))}.");

            // process all classes
            var incidents = new List<IncidentForLog>();
            foreach (var cls in activeClasses)
            {
                var classId = cls.ClassId;
                try
                {
                    tracer.Info($"Processing of class {cls.ClassId}");

                    switch (parameters.Type)
                    {
                        // class daily reports
                        case ActiveClassReportJobType.CoordinatorCrfDailyReport:

                            // generates brand new reports                          
                            var classFileIds = reportService.GenerateClassReportsForDailyReports(rootPath, classId);

                            // prepares data for email and send email to coordinator                   
                            var attachmentFileIds = mainFileService.GetFileIdsByClassFileIds(classFileIds);
                            var attachments = emailAttachmentProviderFactory.CreateSimpleEmailAttachmentProvider(attachmentFileIds.ToArray());
                            classEmailService.SendClassTextMapEmailByTypeToRecipient(EmailTypeEnum.ClassDailyReports, classId,
                                new Dictionary<string, object>(), RecipientType.Coordinator, attachments);
                            break;


                        // report type was not loaded
                        default:
                            throw new ArgumentOutOfRangeException(nameof(ActiveClassReportJobType), $"Unknown active class report job type {parameters.Type}.");
                    }
                }
                catch (Exception e)
                {
                    tracer.Error(e, "Class causes error.");
                    var incident = IncidentForLog
                        .New(IncidentTypeEnum.JobRunError, e)
                        .Entity(EntityTypeEnum.MainClass, cls.ClassId);
                    incidents.Add(incident);
                }
            }

            // process incidents to result
            var result = new JobRunExecutorResult();
            if (incidents.Count > 0)
            {
                result.IsSuccess = false;
                result.Incident = IncidentForLog
                    .New(IncidentTypeEnum.JobRunError, "ActiveClassReportJobExecutor encounters problems", "See inner incidents...")
                    .Entity(EntityTypeEnum.CoreJobRun, jobRun.JobId)
                    .AddChildrenCollection(incidents);
            }
            return result;
        }

        #region private methods

        // parses and validates job run parameters from json params
        private void SetJobRunParameters(string jobParams)
        {            
            try
            {
                parameters = JsonConvert.DeserializeObject<ActiveClassReportJobParameters>(jobParams);

                if (parameters.Type == default(ActiveClassReportJobType))
                    throw new ArgumentException("Type of active class report job is not defined");
            }
            catch (Exception e)
            {
                tracer.Error(e, "JsonParameters of active class report job are invalid.");
                throw;
            }            
        }

        #endregion
    }
}
