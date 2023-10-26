using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Connectors.Integration.Model;
using PerfectlyMadeInc.WebEx.Model;

namespace PerfectlyMadeInc.WebEx.Connectors.AppLogic
{

    /// <summary>
    /// WebEx convertor
    /// </summary>
    public class WebExPersonConvertor: IWebExPersonConvertor
    {

        // converts class registration to WebEx person
        public WebExPersonExtended ConvertToWebExPerson(IntegrationState integrationState)
        {                       
            var result = new WebExPersonExtended();
            result.PersonInfo = new WebExPerson
            {
                FirstName = integrationState.FirstName,
                LastName = integrationState.LastName,
                Street = integrationState.Street,
                Street2 = integrationState.Street2,
                City = integrationState.City,
                State = integrationState.State,
                Country = integrationState.Country,
                ZipCode = integrationState.ZipCode,
                Email = integrationState.Email
            };
            return result;
        }

        // converts WebEx person to WebEx integration state
        public IntegrationState ConvertToIntegrationState(WebExPersonExtended webExPersonExtended)
        {
            var webExPerson = webExPersonExtended.PersonInfo;
            var result = new IntegrationState
            {
                IntegrationStateTypeId = IntegrationStateTypeEnum.InWebEx,
                AttendeeId = webExPerson.AttendeeId,

                FirstName = webExPerson.FirstName,
                LastName = webExPerson.LastName,
                Street = webExPerson.Street,
                Street2 = webExPerson.Street2,
                City = webExPerson.City,
                State = webExPerson.State,
                Country = webExPerson.Country,
                ZipCode = webExPerson.ZipCode,
                Email = webExPerson.Email,                              
            };
            return result;
        }

        // converts WebEx person to empty integration state
        public IntegrationState ConvertToEmptyIntegrationState(long? attendeeId)
        {
            var result = new IntegrationState
            {
                IntegrationStateTypeId = IntegrationStateTypeEnum.NotInWebEx,               
                AttendeeId = attendeeId,
            };
            return result;
        }

        // converts WebEx person to error integration state
        public IntegrationState ConvertToErrorIntegrationState(long? attendeeId, string errorMessage)
        {
            var result = new IntegrationState
            {
                IntegrationStateTypeId = IntegrationStateTypeEnum.Error,                
                AttendeeId = attendeeId,
                ErrorMessage = errorMessage,
            };
            return result;
        }
       
    }

}
