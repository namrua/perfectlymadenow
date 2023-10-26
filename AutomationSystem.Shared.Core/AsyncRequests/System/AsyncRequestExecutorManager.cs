using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.AsyncRequests.System.Models;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Core.AsyncRequests.Data;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.AsyncRequests.System
{
    /// <summary>
    /// Manages async executors
    /// </summary>
    public class AsyncRequestExecutorManager : IAsyncRequestExecutorManager
    {

        // private constants
        private const int AsyncRequestExpirationInSeconds = 120;

        // private fields
        private readonly Dictionary<AsyncRequestTypeEnum, IAsyncRequestExecutorFactory> executorFactoriesMap;

        // private components
        private readonly IAsyncDatabaseLayer asyncDb;
        private readonly IIncidentLogger incidentLogger;
        private readonly ITracerFactory tracerFactory;


        // constructor
        public AsyncRequestExecutorManager(
            IAsyncDatabaseLayer asyncDb,
            IEnumerable<IAsyncRequestExecutorFactory> executorFactories,
            IIncidentLogger incidentLogger,
            ITracerFactory tracerFactory)
        {
            this.asyncDb = asyncDb;
            this.incidentLogger = incidentLogger;
            this.tracerFactory = tracerFactory;

            executorFactoriesMap = CreateExecutorFactoriesMap(executorFactories);
        }

        // executes request synchronously
        public AsyncRequestExecutorResult Execute(long asyncRequestId)
        {
            var runTracer = tracerFactory.CreateTracer<AsyncRequestExecutorManager>(asyncRequestId);
            runTracer.Info("Async request was started synchronously");
            var result = new AsyncRequestExecutorResult();
            IAsyncRequestExecutor executor = null;
            try
            {
                // loads async request
                bool wasStarted;
                var asyncRequest = asyncDb.GetAsyncRequestToProcessing(asyncRequestId, out wasStarted);
                runTracer.Info($"Async request fetched for processing. wasStarted = {wasStarted}");

                // if processing not started, async request is skipped or end with error
                if (!wasStarted)
                {
                    CheckAsyncRequestForException(asyncRequest, asyncRequestId);
                    result.IsSkipped = true;
                    return result;                  
                }
               
                // creates executor
                executor = GetExecutor(asyncRequest.AsyncRequestTypeId);
                runTracer.Info($"Executor was created for request type {asyncRequest.AsyncRequestTypeId}");

                // executes request
                result = executor.Execute(asyncRequest);
                runTracer.Info($"Async request was executed: isSuccess = {result.IsSuccess}");
            }
            catch (Exception e)
            {
                runTracer.Error(e, "Execution of async request caused exception");
                result = new AsyncRequestExecutorResult(e, IncidentTypeEnum.AsyncRequestError, EntityTypeEnum.CoreAsyncRequest, asyncRequestId);                
            }
            finally
            {                
                try
                {
                    // skipped async request is simply unchanged
                    if (!result.IsSkipped)
                    {
                        if (result.IsSuccess)
                        {
                            // change state to finished
                            asyncDb.FinishAsyncRequest(asyncRequestId, ProcessingStateEnum.Finished, result.JsonResult);
                            runTracer.Info("Async request was sucessfully finished");
                        }
                        else
                        {
                            // change state to error and logs incident
                            asyncDb.FinishAsyncRequest(asyncRequestId, ProcessingStateEnum.Error, result.JsonResult);
                            runTracer.Warning("Async request was terminated with error");
                            LogIncident(result, executor);
                            
                        }
                    }
                    else
                    {
                        runTracer.Info("Async request was skipped");
                    }
                }
                catch (Exception e)
                {
                    runTracer.Error(e, "Finally block of Execution method caused exception");
                }
            }
            return result;
        }

        
        // executes request asynchronously
        public Task<AsyncRequestExecutorResult> ExecuteAsync(long asyncRequestId)
        {             
            var result = Task.Run(() => Execute(asyncRequestId));            
            return result;
        }


        #region private mehtods

        private Dictionary<AsyncRequestTypeEnum, IAsyncRequestExecutorFactory> CreateExecutorFactoriesMap(IEnumerable<IAsyncRequestExecutorFactory> executorFactories)
        {
            var result = new Dictionary<AsyncRequestTypeEnum, IAsyncRequestExecutorFactory>();
            foreach (var executorFactory in executorFactories)
            {
                foreach (var asyncRequestTypeId in executorFactory.SupportedAsyncRequestTypes)
                {
                    result.Add(asyncRequestTypeId, executorFactory);
                }
            }

            return result;
        }

        // check async request status, determines whether async task is skipped
        private void CheckAsyncRequestForException(AsyncRequest asyncRequest, long asyncRequestId)
        {
            if (asyncRequest == null)
                throw new ArgumentException($"There are no async request with id {asyncRequestId}");
            if (!asyncRequest.Active)
                throw new InvalidOperationException($"Async request with id {asyncRequestId} is not active");
            if (asyncRequest.ProcessingStateId == ProcessingStateEnum.InProcess 
                && asyncRequest.Started.HasValue 
                && asyncRequest.Started.Value.AddSeconds(AsyncRequestExpirationInSeconds) < DateTime.Now)
                throw new Exception($"Asynch request with id {asyncRequestId} is expired");                        
        }

        // gets executor
        private IAsyncRequestExecutor GetExecutor(AsyncRequestTypeEnum asyncRequestTypeId)
        {
            // loads executor  
            if (!executorFactoriesMap.TryGetValue(asyncRequestTypeId, out var executorFactory))
            {
                throw new ArgumentException(
                    $"There is no registered AsyncRequestExecutor creator with type {asyncRequestTypeId}");
            }

            var result = executorFactory.CreateAsyncRequestExecutor();
            return result;
        }

        // logs incident
        private void LogIncident(AsyncRequestExecutorResult result, IAsyncRequestExecutor executor)
        {
            // creates incident
            var incident = IncidentForLog
                .New(result.IncidentTypeId ?? IncidentTypeEnum.AsyncRequestError,
                    result.IncidentException)
                .Entity(result.IncidentEntityTypeId, result.IncidentEntityId);

            // set as non reportable by executor.CanReportIncident flag
            if (executor != null && !executor.CanReportIncident)
                incident.NonReportable();

            // logs incident
            incidentLogger.LogIncident(incident);
        }

        #endregion

    }

}
