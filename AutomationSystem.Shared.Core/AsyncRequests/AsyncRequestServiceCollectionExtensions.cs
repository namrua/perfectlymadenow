using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Core.AsyncRequests.Data;
using AutomationSystem.Shared.Core.AsyncRequests.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.AsyncRequests
{
    public static class AsyncRequestServiceCollectionExtensions
    {
        public static IServiceCollection AddAsyncRequestServices(this IServiceCollection services)
        {
            // data
            services.AddSingleton<IAsyncDatabaseLayer, AsyncDatabaseLayer>();

            // system
            services.AddSingleton<IAsyncRequestExecutorManagerFactory, AsyncRequestExecutorManagerFactory>();
            services.AddSingleton<ICoreAsyncRequestManager, CoreAsyncRequestManager>();

            return services;
        }
    }
}