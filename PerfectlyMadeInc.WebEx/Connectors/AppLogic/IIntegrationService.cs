using System.Collections.Generic;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{
    /// <summary>
    /// Provides integration operation with WebEx
    /// </summary>
    public interface IIntegrationService
    {

        // gets webex state of person by attendee id
        IntegrationState GetWebExState(long? attendeeId, long sessionId, string errorMessage = null);

        // gets webex state of persons by event's session id
        List<IntegrationState> GetWebExStatesBySessionId(long sessionId);

        // Adds or updatates person in webex, returns attendeeId
        WebExServiceResult<long?> AddUpdatePerson(IntegrationState newState, IntegrationState historyState, long sessionId, bool forceEmailInvitation = false);

        // Remove person from webex
        WebExServiceResult<bool> RemovePerson(long attendeeId);

    }
}
