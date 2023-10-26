using AutomationSystem.Shared.Contract.Incidents.System.Models;

namespace AutomationSystem.Shared.Contract.Incidents.System
{
    /// <summary>
    /// Manages logging of incidents
    /// </summary>
    public interface IIncidentLogger
    {
        long? LogIncident(IncidentForLog incident);
    }
}
