using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.Convertors
{
    public interface IWebExRegistrationConvertor
    {
        
        IntegrationStateDto ConvertToIntegrationState(ClassRegistration registration, long? attendeeId = null);
        
        IntegrationStateDto CreateEmptyIntegrationState(long? attendeeId = null);        
        
        IntegrationStateTypeEnum ResolveIntegrationStateTypeByRegistration(ClassRegistration registration);

    }

}
