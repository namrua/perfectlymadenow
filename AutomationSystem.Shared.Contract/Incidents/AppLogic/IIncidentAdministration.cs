using AutomationSystem.Shared.Contract.Incidents.AppLogic.Models;

namespace AutomationSystem.Shared.Contract.Incidents.AppLogic
{
    /// <summary>
    /// Service for incident managing form administration
    /// </summary>
    public interface IIncidentAdministration
    {
        IncidentsForList GetIncidentsByFilter(IncidentFilter filter, bool search);

        IncidentDetail GetIncidentById(long incidentId);
        
        void ResolveIncident(long incidentId);

        void ReportIncident(long incidentId);
    }
}
