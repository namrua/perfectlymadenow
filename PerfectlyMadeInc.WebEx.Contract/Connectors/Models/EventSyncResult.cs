using System;
using System.Collections.Generic;
using System.Linq;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace PerfectlyMadeInc.WebEx.Contract.Connectors.Models
{
    // Sync result of event
    public class EventSyncResult
    {

        public long EventId { get; set; }
        public string EventName { get; set; }
        public Exception Exception { get; set; }

        public Dictionary<string, List<IntegrationStateDto>> DuplicitEmails { get; set; }
        public List<SyncActionResult> SyncActions { get; set; }
        public List<IntegrationStateDto> UnknownInWebEx { get; set; }
        public List<ConsistencyResult> Inconsistent { get; set; }
        public List<IntegrationStateDto> ErrorStates { get; set; }


        // determines whether there was error in the sync action
        public bool WasError => Exception != null || SyncActions.Any(x => x.WasError);

        // determines whether the was some issue
        public bool HasIssue => DuplicitEmails.Any() || SyncActions.Any() || UnknownInWebEx.Any()
                                || Inconsistent.Any() || ErrorStates.Any() || WasError;

        // constructor
        public EventSyncResult()
        {
            DuplicitEmails = new Dictionary<string, List<IntegrationStateDto>>();
            SyncActions = new List<SyncActionResult>();
            UnknownInWebEx = new List<IntegrationStateDto>();
            Inconsistent = new List<ConsistencyResult>();
            ErrorStates = new List<IntegrationStateDto>();
        }

    }
}
