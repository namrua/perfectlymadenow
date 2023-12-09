using PerfectlyMadeInc.WebEx.Connectors.Integration;
using System.Threading.Tasks;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{
    /// <summary>
    /// WebEx integration factory
    /// </summary>
    public interface IWebExFactory
    {

        // get webEx provider by account id
        IWebExProvider CreateWebExProvider(long accountId);

        // get webEx service by account id
        IIntegrationService CreateIntegrationService(long accountId);
        IWebExProvider GetWebexAccessToken(long accountId);
    }
}
