using AutoMapper;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Core.AsyncRequests;
using AutomationSystem.Shared.Core.BatchUploads;
using AutomationSystem.Shared.Core.ConferenceAccounts;
using AutomationSystem.Shared.Core.Emails;
using AutomationSystem.Shared.Core.EntityIntegration;
using AutomationSystem.Shared.Core.Enums;
using AutomationSystem.Shared.Core.ExcelConnector;
using AutomationSystem.Shared.Core.Files;
using AutomationSystem.Shared.Core.Identities;
using AutomationSystem.Shared.Core.Incidents;
using AutomationSystem.Shared.Core.Jobs;
using AutomationSystem.Shared.Core.Localisation;
using AutomationSystem.Shared.Core.Maintenance;
using AutomationSystem.Shared.Core.Payment;
using AutomationSystem.Shared.Core.Preferences;
using AutomationSystem.Shared.Core.TimeZones;
using AutomationSystem.Shared.Core.WordWorkflows;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
namespace AutomationSystem.Shared.Core
{
    public static class SharedServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddAsyncRequestServices();
            services.AddBatchUploadServices();
            services.AddConferenceAccountServices();
            services.AddEmailServices();
            services.AddEntityIntegrationServices();
            services.AddEnumServices();
            services.AddExcelConnectorServices();
            services.AddFileServices();
            services.AddIdentityServices();
            services.AddIncidentServices();
            services.AddJobServices();
            services.AddLocalisationServices();
            services.AddMaintenanceServices();
            services.AddPaymentServices();
            services.AddPreferenceServices();
            services.AddTimeZoneServices();
            services.AddWordWorkflowServices();

            // adds mappers
            services.AddSingleton<ICoreMapper>(sp =>
            {
                var profiles = new List<Profile>();
                profiles.AddRange(IncidentServiceCollectionExtensions.CreateProfiles());
                profiles.AddRange(BatchUploadServiceCollectionExtensions.CreateProfiles());
                profiles.AddRange(IdentityServiceCollectionExtensions.CreateProfiles());
                
                var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfiles(profiles);
                });
                var mapper = new Mapper(mapperConfiguration);

                return new CoreMapper(mapper);
            });

            return services;
        }
    }

}
