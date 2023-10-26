using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Incidents.AppLogic;
using AutomationSystem.Shared.Contract.Incidents.System;
using AutomationSystem.Shared.Core.Incidents.AppLogic;
using AutomationSystem.Shared.Core.Incidents.Data;
using AutomationSystem.Shared.Core.Incidents.System;
using AutomationSystem.Shared.Core.Incidents.System.AsyncRequestExecutors;
using AutomationSystem.Shared.Core.Incidents.System.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.Incidents
{
    public static class IncidentServiceCollectionExtensions
    {
        public static IServiceCollection AddIncidentServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IIncidentAdministration, IncidentAdministration>();
            
            // data
            services.AddSingleton<IIncidentDatabaseLayer, IncidentDatabaseLayer>();

            // system
            services.AddSingleton<IIncidentLogger, IncidentLogger>();

            // system - async request executors
            services.AddSingleton<IAsyncRequestExecutorFactory, EmailIncidentReportingExecutorFactory>();

            return services;
        }

        public static List<Profile> CreateProfiles()
        {
            return new List<Profile>
            {
                new IncidentProfile()
            };
        }
    }
}   