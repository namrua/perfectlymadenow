using AutomationSystem.Shared.Contract.Maintenance.System;
using AutomationSystem.Shared.Core.Maintenance.Data;
using AutomationSystem.Shared.Core.Maintenance.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.Maintenance
{
    public static class MaintenanceServiceCollectionExtensions
    {
        public static IServiceCollection AddMaintenanceServices(this IServiceCollection services)
        {
            // data
            services.AddSingleton<IMaintenanceDatabaseLayer, MaintenanceDatabaseLayer>();

            // system
            services.AddSingleton<ICoreMaintenanceService, CoreMaintenanceService>();

            return services;
        }
    }
}
