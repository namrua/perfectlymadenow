using AutomationSystem.Shared.Contract.Incidents.System.Models;

namespace AutomationSystem.Shared.Contract.Incidents.System
{
    public interface IIncidentHandler
    {
        string HandlerCode { get; }
        bool CanHandle(IncidentForLog incident);
        IncidentOperationType HandleIncident(IncidentForLog incident);
    }
}
