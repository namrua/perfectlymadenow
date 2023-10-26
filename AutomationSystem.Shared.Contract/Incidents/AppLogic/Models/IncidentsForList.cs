using System.Collections.Generic;

namespace AutomationSystem.Shared.Contract.Incidents.AppLogic.Models
{
    /// <summary>
    /// Incidents for list
    /// </summary>
    public class IncidentsForList
    {

        public IncidentFilter Filter { get; set; }
        public List<IncidentDetail> Items { get; set; }
        public bool WasSearched { get; set; }

        // constructor
        public IncidentsForList(IncidentFilter filter = null)
        {
            Filter = filter ?? new IncidentFilter();
            Items = new List<IncidentDetail>();
        }

    }
}
