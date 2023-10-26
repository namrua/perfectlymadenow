using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Model;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Main.Core.Integration.System.NoIntegration
{
    /// <summary>
    /// Handles registration integratino for no integration
    /// </summary>
    public class NoIntegrationRequestHandler : IRegistrationIntegrationRequestHandler
    {
        private readonly ITracerFactory tracerFactory;

        public NoIntegrationRequestHandler(ITracerFactory tracerFactory)
        {
            this.tracerFactory = tracerFactory;
        }

        // handles request 
        public void Handle(AsyncRequest request, ClassRegistration registration)
        {
            var tracer = this.tracerFactory.CreateTracer<NoIntegrationRequestHandler>(request.AsyncRequestId);
            tracer.Info("Class registration has no integration.");
        }
    }
}
