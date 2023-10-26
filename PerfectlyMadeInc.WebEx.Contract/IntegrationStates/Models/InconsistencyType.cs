
namespace PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models
{
    /// <summary>
    /// Determines types of inconsistencies
    /// </summary>
    public enum InconsistencyType
    {
        None,
        NotInWebEx,
        NotInSystem,
        InconsistentData,
        DuplicitEmail,
    }
}
