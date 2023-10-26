using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.WebEx.Contract.IntegrationStates;
using PerfectlyMadeInc.WebEx.IntegrationStates.Data;

namespace PerfectlyMadeInc.WebEx.IntegrationStates
{
    public static class IntegrationStateServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationStateServices(this IServiceCollection services)
        {
            services.AddSingleton<IIntegrationAdministration, IntegrationAdministration>();
            services.AddSingleton<IIntegrationDatabaseLayer, IntegrationDatabaseLayer>();
            return services;
        }
    }
}