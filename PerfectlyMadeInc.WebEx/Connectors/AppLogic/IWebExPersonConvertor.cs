using PerfectlyMadeInc.WebEx.Connectors.Integration.Model;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{
    /// <summary>
    /// WebEx person convertor
    /// </summary>
    public interface IWebExPersonConvertor
    {

        // converts class registration to WebEx person
        WebExPersonExtended ConvertToWebExPerson(IntegrationState integrationState);

        // converts WebEx person to WebEx integration state
        IntegrationState ConvertToIntegrationState(WebExPersonExtended webExPerson);

        // converts WebEx person to empty integration state
        IntegrationState ConvertToEmptyIntegrationState(long? attendeeId);

        // converts WebEx person to error integration state
        IntegrationState ConvertToErrorIntegrationState(long? attendeeId, string errorMessage);

    }
}
