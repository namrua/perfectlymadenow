using System;
using System.Collections.Generic;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{
    /// <summary>
    /// Pairs integration states with misc strategies
    /// </summary>
    public interface IIntegrationStatePairingProvider
    {

        // // pairs integration states by common matching patterns 
        PairingResult<IntegrationState> Pair(List<IntegrationState> left, List<IntegrationState> right,
            params Func<IntegrationState, IntegrationState>[] matchingPatterns);


        // pairs integration states by same entity 
        PairingResult<IntegrationState> PairByEntity(List<IntegrationState> left, List<IntegrationState> right,
            Func<IntegrationState, IntegrationState> noMatchSubstitution = null);

        // pairs integration states by attendeeId
        PairingResult<IntegrationState> PairByAttendeeId(List<IntegrationState> left, List<IntegrationState> right,
            Func<IntegrationState, IntegrationState> noMatchSubstitution = null);

        // pairs integration states by attendeeId or email for InSystem states
        PairingResult<IntegrationState> PairByAttendeeIdOrEmailForInSystem(List<IntegrationState> left, List<IntegrationState> right,
            Func<IntegrationState, IntegrationState> noMatchSubstitution = null);

    }
}
