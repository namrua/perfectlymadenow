using System;
using System.Collections.Generic;
using System.Linq;

namespace PerfectlyMadeInc.WebEx.Contract.Connectors.Models
{
    // Sync result of program
    public class ProgramSyncResult
    {

        public long ProgramId { get; set; }
        public string ProgramName { get; set; }
        public Exception Exception { get; set; }

        public List<EventSyncResult> EventResults { get; set; }


        // determines whether there was error in the sync action
        public bool WasError => Exception != null || EventResults.Any(x => x.WasError);


        // constructor
        public ProgramSyncResult()
        {
            EventResults = new List<EventSyncResult>();
        }

    }
}
