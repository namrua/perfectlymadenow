using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace PerfectlyMadeInc.WebEx.Contract.IntegrationStates
{
    /// <summary>
    /// Provides information for webex integration administration
    /// </summary>
    public interface IIntegrationAdministration
    {

        // gets web ex integration state summary
        List<IntegrationStateSummary> GetIntegrationStateSummary(IntegrationStateDto systemState, EntityTypeEnum entityTypeId, long entityId, long programId);

    }
}
