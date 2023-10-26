using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates.Models;
using System.Collections.Generic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Integration;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic
{
    public interface IRegistrationIntegrationAdministration
    {
        RegistrationIntegrationPageModel GetRegistrationIntegrationPageModel(long registrationId);

        // todo: #Integration - make generics, now it is WebEx or NoIntegration specific
        // executes integration request (update, sync, send invitation)
        List<IntegrationStateSummary> ExecuteIntegrationRequest(long registrationId, AsyncRequestTypeEnum integrationRequestType);
    }
}
