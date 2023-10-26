using System;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Incidents.AppLogic;
using AutomationSystem.Shared.Contract.Incidents.AppLogic.Models;
using AutomationSystem.Shared.Core.Incidents.Data;

namespace AutomationSystem.Shared.Core.Incidents.AppLogic
{
    /// <summary>
    /// Service for incident managing form administration
    /// </summary>
    public class IncidentAdministration : IIncidentAdministration
    {
        private readonly IIncidentDatabaseLayer incidentDb;
        private readonly ICoreAsyncRequestManager asyncRequestManager;
        private readonly ICoreMapper coreMapper;

        public IncidentAdministration(
            IIncidentDatabaseLayer incidentDb,
            ICoreAsyncRequestManager asyncRequestManager,
            ICoreMapper coreMapper)
        {
            this.incidentDb = incidentDb;
            this.coreMapper = coreMapper;
            this.asyncRequestManager = asyncRequestManager;
        }

        public IncidentsForList GetIncidentsByFilter(IncidentFilter filter, bool search)
        {
            var result = new IncidentsForList(filter)
            {
                WasSearched = search
            };
            
            if (search)
            {
                var incidents = incidentDb.GetIncidentsByFilter(filter);
                result.Items = incidents.Select(coreMapper.Map<IncidentDetail>).ToList();
            }          
            
            return result;
        }

        public IncidentDetail GetIncidentById(long incidentId)
        {
            var incident = incidentDb.GetIncidentById(incidentId);
            if (incident == null)
            {
                throw new ArgumentException($"There is no Incident with id {incidentId}.");
            }

            var result = coreMapper.Map<IncidentDetail>(incident);
            return result;
        }
        
        public void ResolveIncident(long incidentId)
        {
            incidentDb.SetIncidentAsResolved(incidentId);
        }

        public void ReportIncident(long incidentId)
        {
            asyncRequestManager.AddReportIncidentRequest(incidentId, (int) SeverityEnum.High);
        }
    }
}
