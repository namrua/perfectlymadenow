using AutomationSystem.Shared.Contract.Incidents.System.Models;

namespace AutomationSystem.Shared.Contract.Jobs.System.Models
{

    /// <summary>
    /// Result of job run executor 
    /// </summary>
    public class JobRunExecutorResult
    {
        
        // public properties
        public bool IsSuccess { get; set; }
        public string JsonResult { get; set; }
        public IncidentForLog Incident { get; set; }

        // constructor
        public JobRunExecutorResult(string jsonResult = null)
        {
            IsSuccess = true;
            JsonResult = jsonResult;
        }

        // constructor
        public JobRunExecutorResult(IncidentForLog incident, string jsonResult = null)
        {
            IsSuccess = false;
            JsonResult = jsonResult;
            Incident = incident;
        }

    }
}
