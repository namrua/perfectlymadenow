using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.WebEx.Contract.Connectors;

namespace AutomationSystem.Main.Core.Integration.System.WebEx
{
    /// <summary>
    /// Handles registration integration request to WebEx
    /// </summary>
    public class WebExRegistrationIntegrationRequestHandler : IRegistrationIntegrationRequestHandler
    {

        private readonly IIntegrationRequestHandler requestHandler;
        private readonly IWebExRegistrationConvertor registrationConvertor;

        public WebExRegistrationIntegrationRequestHandler(IIntegrationRequestHandler requestHandler, IWebExRegistrationConvertor registrationConvertor)
        {
            this.requestHandler = requestHandler;
            this.registrationConvertor = registrationConvertor;
        }
        
        public void Handle(AsyncRequest request, ClassRegistration registration)
        {
            var cls = registration.Class;
            if (cls.IntegrationTypeId != IntegrationTypeEnum.WebExProgram || !cls.IntegrationEntityId.HasValue)
                throw new ArgumentException($"There is invalid integration type {cls.IntegrationTypeId} or null integration entity id.");
            
            var systemState = registrationConvertor.ConvertToIntegrationState(registration);
            requestHandler.Initialize(request.AsyncRequestTypeId, systemState);
            requestHandler.HandleForProgram(cls.IntegrationEntityId.Value);
        }
    }
}
