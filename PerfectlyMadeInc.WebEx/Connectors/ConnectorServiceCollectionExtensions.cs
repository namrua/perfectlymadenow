using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.WebEx.Connectors.AppLogic;
using PerfectlyMadeInc.WebEx.Contract.Connectors;

namespace PerfectlyMadeInc.WebEx.Connectors
{
    public static class ConnectorServiceCollectionExtensions
    {
        public static IServiceCollection AddConnectorServices(this IServiceCollection services)
        {
            services.AddTransient<IIntegrationRequestHandler, IntegrationRequestHandler>();
            services.AddTransient<IIntegrationSyncExecutor, IntegrationSyncExecutor>();
            services.AddSingleton<IWebExFactory, WebExFactory>();
            return services;
        }
    }
}