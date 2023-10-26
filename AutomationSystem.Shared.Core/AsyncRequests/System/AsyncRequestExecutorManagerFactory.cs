using System;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Core.AsyncRequests.Data;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.AsyncRequests.System
{
    /// <summary>
    /// Factory for AsyncRequestExecutorManager
    /// </summary>
    public class AsyncRequestExecutorManagerFactory : IAsyncRequestExecutorManagerFactory
    {
        // late evaluation of async request executor manager breaks cyclical dependency on IIncidentLogger and other classes
        private readonly Lazy<IAsyncRequestExecutorManager> asyncRequestExecutorManager;

        public AsyncRequestExecutorManagerFactory(IServiceProvider serviceProvider)
        {
            asyncRequestExecutorManager = new Lazy<IAsyncRequestExecutorManager>(() => new AsyncRequestExecutorManager(
                serviceProvider.GetService<IAsyncDatabaseLayer>(),
                serviceProvider.GetServices<IAsyncRequestExecutorFactory>(),
                serviceProvider.GetService<IIncidentLogger>(),
                serviceProvider.GetService<ITracerFactory>()));
        }

        public IAsyncRequestExecutorManager CreateAsyncRequestExecutorManager()
        {
            return asyncRequestExecutorManager.Value;
        }
    }

}
