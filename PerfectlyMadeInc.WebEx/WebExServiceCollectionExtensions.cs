using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.WebEx.Accounts;
using PerfectlyMadeInc.WebEx.Connectors;
using PerfectlyMadeInc.WebEx.IntegrationStates;
using PerfectlyMadeInc.WebEx.Programs;

namespace PerfectlyMadeInc.WebEx
{
    public static class WebExServiceCollectionExtensions
    {
        public static IServiceCollection AddWebExServices(this IServiceCollection services)
        {
            services.AddAccountServices();
            services.AddConnectorServices();
            services.AddIntegrationStateServices();
            services.AddProgramServices();
            return services;
        }
    }
}
