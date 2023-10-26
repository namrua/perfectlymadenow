using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Incidents.System.Models;

namespace AutomationSystem.Main.Core.Integration.System.Models
{
    /// <summary>
    /// Result of Registration integration synchronization
    /// </summary>
    public class RegistrationIntegrationSyncResult
    {
        public List<IncidentForLog> Incidents { get; set; }
        public string ReportContent { get; set; }
        public bool SendReport { get; set; }
        
        public RegistrationIntegrationSyncResult()
        {
            SendReport = true;
            Incidents = new List<IncidentForLog>();            
        }
    }
}
