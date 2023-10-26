using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;
using AutomationSystem.Shared.Core.Incidents.Data;
using AutomationSystem.Shared.Model;
using ClosedXML.Excel;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Incidents.System
{
    /// <summary>
    /// Manages logging of incidents
    /// </summary>
    public class IncidentLogger : IIncidentLogger
    {
        private readonly ITracer tracer;
        private readonly IIncidentDatabaseLayer incidentDb;
        private readonly ICoreAsyncRequestManager asyncManager;
        private readonly ICoreMapper coreMapper;
        private readonly List<IIncidentHandler> incidentHandlers;

        public IncidentLogger(
            IIncidentDatabaseLayer incidentDb,
            ICoreAsyncRequestManager asyncManager,
            ICoreMapper coreMapper,
            ITracerFactory tracerFactory,
            IEnumerable<IIncidentHandler> incidentHandlers)
        {
            this.incidentDb = incidentDb;
            this.asyncManager = asyncManager;
            this.coreMapper = coreMapper;
            this.incidentHandlers = incidentHandlers.ToList();
            tracer = tracerFactory.CreateTracer<IncidentLogger>();
        }

        public long? LogIncident(IncidentForLog incident)
        {
            if (incident == null)
            {
                throw new ArgumentNullException(nameof(incident));
            }

            var incidentToInsert = coreMapper.Map<Incident>(incident);

            var incidentHandler = incidentHandlers.FirstOrDefault(x => x.CanHandle(incident));
            if (incidentHandler != null)
            {
                incidentToInsert.HandlerCode = incidentHandler.HandlerCode;
                var operationType = incidentHandler.HandleIncident(incident);

                switch (operationType)
                {
                    case IncidentOperationType.ProcessAsHidden:
                        incidentToInsert.CanBeReport = false;
                        incidentToInsert.IsHidden = true;
                        incidentToInsert.IncidentChildren.ForEach(x => x.IsHidden = true);
                        break;

                    case IncidentOperationType.Ignore:
                        tracer.Info($"Incident was ignored: {incident.Message}");
                        return null;
                }
            }

            var incidentId = incidentDb.InsertIncident(incidentToInsert);

            if (incidentToInsert.CanBeReport)
            {
                asyncManager.AddReportIncidentRequest(incidentId, (int) SeverityEnum.High);
            }

            tracer.Warning($"New incident with id {incidentId} was created: {incident.Message}");
            return incidentId;
        }
    }
}
