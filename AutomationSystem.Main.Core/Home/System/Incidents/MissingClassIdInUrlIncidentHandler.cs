using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Contract.Incidents.System.Models;

namespace AutomationSystem.Main.Core.Home.System.Incidents
{
    public class MissingClassIdInUrlIncidentHandler : IIncidentHandler
    {
        public string HandlerCode => "MissingClassIdInUrl";

        public bool CanHandle(IncidentForLog incident)
        {
            var result = incident.Message.StartsWith("The parameters dictionary contains a null entry for parameter 'classId'");
            return result;
        }

        public IncidentOperationType HandleIncident(IncidentForLog incident)
        {
            return IncidentOperationType.ProcessAsHidden;
        }
    }
}
