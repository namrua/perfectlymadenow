using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.IntegrationStates.Data.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.IntegrationStates.Data
{
    /// <summary>
    /// Provides integration database layer
    /// </summary>
    public interface IIntegrationDatabaseLayer
    {

        // gets WebEx integration state types
        List<IntegrationStateType> GetIntegrationStateTypes();

        // gets list of webex integration state by entity id 
        List<IntegrationState> GetIntegrationStatesByEntityId(EntityTypeEnum entityTypeId, long entityId, IntegrationStateIncludes includes = IntegrationStateIncludes.None);

        // gets all integration states by eventId
        List<IntegrationState> GetIntegrationStatesByEventId(long eventId, IntegrationStateIncludes includes = IntegrationStateIncludes.None);

        // get WebEx integration state
        IntegrationState GetIntegrationStateByEntityId(EntityTypeEnum entityTypeId, long entityId, long eventId, IntegrationStateIncludes includes = IntegrationStateIncludes.None);

        
        // insert WebEx integration state
        long InsertIntegrationState(IntegrationState state);

        // update WebEx integration state
        void UpdateIntegrationState(IntegrationState state);
        
    }
}
