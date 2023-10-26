using PerfectlyMadeInc.WebEx.Contract.Connectors.Models;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{
    /// <summary>
    /// WebEx consistency resolver
    /// </summary>
    public interface IConsistencyResolver
    {

        // compares two integration states
        ConsistencyResult Compare(IntegrationState systemState, IntegrationState webExState);

    }
}
