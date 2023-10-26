using AutoMapper;
using AutomationSystem.Main.Core.Addresses.AppLogic;
using AutomationSystem.Main.Core.Certificates;
using AutomationSystem.Main.Core.Classes;
using AutomationSystem.Main.Core.DistanceClassTemplates;
using AutomationSystem.Main.Core.DistanceProfiles;
using AutomationSystem.Main.Core.Emails;
using AutomationSystem.Main.Core.Enums;
using AutomationSystem.Main.Core.FileServices;
using AutomationSystem.Main.Core.FormerClasses;
using AutomationSystem.Main.Core.Home;
using AutomationSystem.Main.Core.Integration;
using AutomationSystem.Main.Core.MainAsyncRequestManagers;
using AutomationSystem.Main.Core.MainEnum;
using AutomationSystem.Main.Core.Maintenance;
using AutomationSystem.Main.Core.MaterialDistribution;
using AutomationSystem.Main.Core.Payment;
using AutomationSystem.Main.Core.Persons;
using AutomationSystem.Main.Core.Preferences;
using AutomationSystem.Main.Core.PriceLists;
using AutomationSystem.Main.Core.Profiles;
using AutomationSystem.Main.Core.Registrations;
using AutomationSystem.Main.Core.Reports;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using AutomationSystem.Main.Contract;
using AutomationSystem.Main.Core.Contacts;
using AutomationSystem.Main.Core.RegistrationUpload;

namespace AutomationSystem.Main.Core
{
    public static class MainServiceCollectionExtensions
    {
        public static IServiceCollection AddMainServices(this IServiceCollection services)
        {
            // services
            services.AddAddressServices();
            services.AddCertificateServices();
            services.AddClassServices();
            services.AddContactServices();
            services.AddDistanceClassTemplateServices();
            services.AddDistanceProfileServices();
            services.AddEmailServices();
            services.AddEnumServices();
            services.AddFileServiceServices();
            services.AddFormerClassesServices();
            services.AddHomeServices();
            services.AddIntegrationServices();
            services.AddMainAsyncRequestManagerServices();
            services.AddMaintenanceServices();
            services.AddMaterialDistributionServices();
            services.AddPaymentServices();
            services.AddPersonServices();
            services.AddPreferenceServices();
            services.AddPriceListServices();
            services.AddProfileServices();
            services.AddRegistrationServices();
            services.AddRegistrationUploadServices();
            services.AddReportingServices();

            // adds mappers
            services.AddSingleton<IMainMapper>(sp =>
            {
                var profiles = new List<Profile>();
                profiles.AddRange(ClassServiceCollectionExtensions.CreateProfiles(sp));
                profiles.AddRange(DistanceClassTemplateServiceCollectionExtensions.CreateProfiles(sp));
                profiles.AddRange(DistanceProfileServiceCollectionExtensions.CreateProfiles());
                profiles.AddRange(MainEnumServiceCollectionExtensions.CreateProfiles(sp));
                profiles.AddRange(PriceListServiceCollectionExtensions.CreateProfiles(sp));
                profiles.AddRange(ProfileServiceCollectionExtensions.CreateProfiles());
                profiles.AddRange(RegistrationServiceCollectionExtensions.CreateProfiles());
                profiles.AddRange(FormerClassesServiceCollectionExtensions.CreateProfiles());
                profiles.AddRange(ContactServiceCollectionExtensions.CreateProfiles());

                var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfiles(profiles);
                });
                var mapper = new Mapper(mapperConfiguration);
                return new MainMapper(mapper);
            });

            return services;
        }
    }
}
