using System;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.AsyncRequests.System.Models
{
    /// <summary>
    /// Asynchronous request executor result
    /// </summary>
    public class AsyncRequestExecutorResult
    {
        
        // public properties
        public bool IsSuccess { get; set; }
        public bool IsSkipped { get; set; }
        public string JsonResult { get; set; }
        public IncidentTypeEnum? IncidentTypeId { get; set; }
        public Exception IncidentException { get; set; }
        public EntityTypeEnum? IncidentEntityTypeId { get; set; }
        public long? IncidentEntityId { get; set; }

        // success constructor
        public AsyncRequestExecutorResult(string jsonResult = null)
        {
            IsSuccess = true;
            JsonResult = jsonResult;
        }

        // fail constructor
        public AsyncRequestExecutorResult(Exception incidentException, IncidentTypeEnum incidentTypeId, 
            EntityTypeEnum? incidentEntityTypeId = null, long? incidentEntityId = null)
        {
            IsSuccess = false;
            IncidentTypeId = incidentTypeId;
            IncidentException = incidentException;
            IncidentEntityTypeId = incidentEntityTypeId;
            IncidentEntityId = incidentEntityId;
        }

    }
}
