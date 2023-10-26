using System.Collections.Generic;
using PerfectlyMadeInc.WebEx.Contract.Connectors.Models;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace PerfectlyMadeInc.WebEx.Contract.Connectors
{
    /// <summary>
    /// Program and event integration sync executor
    /// </summary>
    public interface IIntegrationSyncExecutor
    {

        // initalizes input data before operation
        void Initialize(List<IntegrationStateDto> systemStates);

        // executes sync for WebEx program
        ProgramSyncResult ExecuteSyncForProgram(long programId);

        // executes sync WebEx event
        EventSyncResult ExecuteSyncForEvent(long eventId);

    }
}
