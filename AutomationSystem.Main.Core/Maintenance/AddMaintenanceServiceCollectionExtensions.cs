using AutomationSystem.Main.Core.Maintenance.Data;
using AutomationSystem.Main.Core.Maintenance.System.JobRunExecutors;
using AutomationSystem.Shared.Contract.Jobs.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.Maintenance
{
    public static class AddMaintenanceServiceCollectionExtensions
    {
        public static IServiceCollection AddMaintenanceServices(this IServiceCollection services)
        {
            // data
            services.AddSingleton<IMaintenanceDatabaseLayer, MaintenanceDatabaseLayer>();

            // system - job executors
            services.AddSingleton<IJobRunExecutorFactory, DatabaseClearingJobFactory>();

            return services;
        }
    }
}
