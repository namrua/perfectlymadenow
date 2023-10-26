using AutomationSystem.Shared.Contract.EntityIntegration.System;
using AutomationSystem.Shared.Core.EntityIntegration.Data;
using AutomationSystem.Shared.Core.EntityIntegration.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.EntityIntegration
{
    public static class EntityIntegrationServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityIntegrationServices(this IServiceCollection services)
        {
            // data
            services.AddSingleton<IIntegrationDatabaseLayer, IntegrationDatabaseLayer>();

            // system
            services.AddSingleton<IGenericEntityIntegrationProvider, GenericEntityIntegrationProvider>();

            return services;
        }
    }
}