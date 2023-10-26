using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Shared.Contract.Incidents.AppLogic.Models;
using AutomationSystem.Shared.Core.Incidents.Data.Extensions;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;

namespace AutomationSystem.Shared.Core.Incidents.Data
{
    /// <summary>
    /// Incident database layer
    /// </summary>
    public class IncidentDatabaseLayer : IIncidentDatabaseLayer
    {
        private const int IncidentSelectTimeout = 600;             // determines timeout in seconds

        public Incident GetIncidentById(long incidentId)
        {
            using (var context = new CoreEntities())
            {
                context.Database.CommandTimeout = IncidentSelectTimeout;

                var result = context.Incidents.Include("IncidentChildren").Include("IncidentType").Include("EntityType")
                    .Active().FirstOrDefault(x => x.IncidentId == incidentId);                             
                return result;
            }
        }
        
        public List<Incident> GetIncidentsByFilter(IncidentFilter filter)
        {
            using (var context = new CoreEntities())
            {
                context.Database.CommandTimeout = IncidentSelectTimeout;

                var result = context.Incidents.Include("IncidentChildren").Include("IncidentType").Include("EntityType")
                    .Filter(filter).OrderByDescending(x => x.Occurred).ToList();
                return result;
            }
        }

        public bool SetIncidentAsResolved(long incidentId)
        {
            using (var context = new CoreEntities())
            {
                var incidentToUpdate = context.Incidents.Active().FirstOrDefault(x => x.IncidentId == incidentId);
                if (incidentToUpdate == null || incidentToUpdate.IsResolved)
                {
                    return false;
                }

                incidentToUpdate.IsResolved = true;
                incidentToUpdate.Resolved = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }

        public void UpdateIncidentReportingState(long incidentId, bool isReported, DateTime? reported, int reportingAttempts)
        {
            using (var context = new CoreEntities())
            {
                var incidentToUpdate = context.Incidents.Active().FirstOrDefault(x => x.IncidentId == incidentId);
                if (incidentToUpdate == null)
                {
                    throw new ArgumentException($"There is no Incident with id {incidentId}");
                }

                incidentToUpdate.IsReported = isReported;
                incidentToUpdate.Reported = reported;
                incidentToUpdate.ReportingAttempts = reportingAttempts;
                context.SaveChanges();
            }
        }

        public long InsertIncident(Incident incident)
        {
            using (var context = new CoreEntities())
            {
                context.Incidents.Add(incident);
                context.SaveChanges();
                return incident.IncidentId;
            }
        }
    }
}

