using AutomationSystem.Shared.Contract.Jobs.AppLogic;
using AutomationSystem.Shared.Contract.Jobs.System;
using AutomationSystem.Shared.Core.Jobs.AppLogic;
using AutomationSystem.Shared.Core.Jobs.Data;
using AutomationSystem.Shared.Core.Jobs.System;
using AutomationSystem.Shared.Core.Jobs.System.JobRunExecutors;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.Jobs
{
    public static class JobServiceCollectionExtensions
    {
        public static IServiceCollection AddJobServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IJobAdministration, JobAdministration>();

            // data
            services.AddSingleton<IJobDatabaseLayer, JobDatabaseLayer>();

            // integration
            // system
            services.AddSingleton<IJobScheduler, JobScheduler>();
            services.AddSingleton<IJobRunExecutorManager, JobRunExecutorManager>();

            // system - job run executors
            services.AddSingleton<IJobRunExecutorFactory, TestJobRunExecutorFactory>();

            return services;
        }
    }
}