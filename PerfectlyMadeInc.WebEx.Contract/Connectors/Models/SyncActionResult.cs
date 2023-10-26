using System;

namespace PerfectlyMadeInc.WebEx.Contract.Connectors.Models
{

    // Encapsulates sync action result
    public class SyncActionResult
    {

        public Exception Exception { get; set; }
        public ConsistencyResult ConsistencyResult { get; set; }

        // determines whether there was error in the sync action
        public bool WasError => Exception != null;

        // constructor
        public SyncActionResult(ConsistencyResult consistencyResult)
        {
            ConsistencyResult = consistencyResult;
        }

    }
}
