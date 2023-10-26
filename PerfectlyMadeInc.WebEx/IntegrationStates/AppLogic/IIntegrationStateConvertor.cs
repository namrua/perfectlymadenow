using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.Connectors.Models;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.IntegrationStates.AppLogic
{
    /// <summary>
    /// WebEx integration state convertor
    /// </summary>
    public interface IIntegrationStateConvertor
    {

        // converts integration state to webex integration status detail
        IntegrationStateDetail ConvertToIntegrationStateDetail(IntegrationStateDto state);

        // converts consistency result to WebEx integration state summary
        IntegrationStateSummary ConvertToIntegrationStateSummary(ConsistencyResult consistencyResult, string eventName);

        // converts IntegrationStateDto to IntegrationState
        IntegrationState ConvertToIntegrationState(IntegrationStateDto integrationStateDto);

        // converts IntegrationState to IntegrationStateDto
        IntegrationStateDto ConvertToIntegrationStateDto(IntegrationState integrationState);


        // todo: #ISP - it is possible to separate working with IntegrationState and GUI converting

        // fills common columns of WebExIntegration state
        void FillIntegrationState(IntegrationState state, long integrationStateId, EntityTypeEnum entityTypeId, long entityId, long eventId);

        // clones integration state
        IntegrationState CloneIntegrationState(IntegrationState state, long? attendeeId = null, long? integrationStateId = null);

        
    }
}
