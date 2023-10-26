using AutomationSystem.Main.Core.MainAsyncRequestManagers.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.MainAsyncRequestManagers
{
    public static class MainAsyncRequestManagerServiceCollectionExtensions
    {
        public static IServiceCollection AddMainAsyncRequestManagerServices(this IServiceCollection services)
        {
            // system
            services.AddSingleton<IMainAsyncRequestManager, MainAsyncRequestManager>();

            return services;
        }
    }
}
