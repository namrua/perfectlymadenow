using System.Collections.Generic;
using System.ComponentModel;

namespace PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models
{ 
    /// <summary>
    /// WebEx integration state summary
    /// </summary>
    public class IntegrationStateSummary
    {

        [DisplayName("Event name")]
        public string EventName { get; set; }

        [DisplayName("Integration state detail")]
        public IntegrationStateDetail Detail { get; set; }

        [DisplayName("Has integration error")]
        public bool HasError { get; set; }

        [DisplayName("Is inconsistent")]
        public bool IsInconsistent { get; set; }

        [DisplayName("Inconsistency type")]
        public InconsistencyType InconsistencyType { get; set; }

        [DisplayName("Inconsistent data fields")]
        public List<InconsistentField> InconsistentFields { get; set; }


        // constructor
        public IntegrationStateSummary()
        {
            InconsistentFields = new List<InconsistentField>();
        }

    }
}
