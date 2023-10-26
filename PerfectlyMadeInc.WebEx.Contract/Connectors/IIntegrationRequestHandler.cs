using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace PerfectlyMadeInc.WebEx.Contract.Connectors
{

    /// <summary>
    /// Program and event integration request handler
    /// </summary>
    public interface IIntegrationRequestHandler
    {

        // initializes input data before operation
        void Initialize(AsyncRequestTypeEnum eventTypeId, IntegrationStateDto systemState);

        // handles integration event for WebEx program
        void HandleForProgram(long programId);

        // handles integration event for WebEx event
        void HandleForEvent(long eventId);

    }

}
