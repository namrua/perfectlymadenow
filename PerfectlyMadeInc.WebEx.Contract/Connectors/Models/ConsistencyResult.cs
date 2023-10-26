using System.Collections.Generic;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace PerfectlyMadeInc.WebEx.Contract.Connectors.Models
{
    /// <summary>
    /// Encapsulates WebEx consistency result
    /// </summary>
    public class ConsistencyResult
    {

        public long? EntityId { get; set; }
        public IntegrationStateDto SystemState { get; set; }
        public IntegrationStateDto WebExState { get; set; }

        public bool IsInconsistent { get; set; }
        public InconsistencyType InconsistencyType { get; set; }
        public List<InconsistentField> InconsistentFields { get; set; }

        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }

        public SyncOperationType OperationType { get; set; }

        // constructor
        public ConsistencyResult()
        {
            IsInconsistent = false;
            InconsistencyType = InconsistencyType.None;
            InconsistentFields = new List<InconsistentField>();
        }

    }
}
