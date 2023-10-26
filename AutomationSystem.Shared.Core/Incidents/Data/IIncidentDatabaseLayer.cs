using System;
using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Incidents.AppLogic.Models;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Incidents.Data
{
    /// <summary>
    /// Incident database layer
    /// </summary>
    public interface IIncidentDatabaseLayer
    {
        Incident GetIncidentById(long incidentId);

        List<Incident> GetIncidentsByFilter(IncidentFilter filter);

        bool SetIncidentAsResolved(long incidentId);

        void UpdateIncidentReportingState(long incidentId, bool isReported, DateTime? reported, int reportingAttempts);
        
        long InsertIncident(Incident incident);
    }

}
