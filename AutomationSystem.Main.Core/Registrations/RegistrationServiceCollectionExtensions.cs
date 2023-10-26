using AutoMapper;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Registrations.AppLogic;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Localisation.System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Registrations.AppLogic.Convertors;
using AutomationSystem.Main.Core.Registrations.AppLogic.Factories;
using AutomationSystem.Main.Core.Registrations.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.Convertors;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Registrations.AppLogic.AsyncRequestExecutors;
using AutomationSystem.Main.Core.Registrations.System.Emails;
using AutomationSystem.Main.Core.Registrations.System.JobRunExecutors;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Shared.Contract.AsyncRequests.System;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Jobs.System;

namespace AutomationSystem.Main.Core.Registrations
{
    public static class RegistrationServiceCollectionExtensions
    {
        public static IServiceCollection AddRegistrationServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IFormerFilterForReviewProvider, FormerFilterForReviewProvider>();
            services.AddSingleton<IRegistrationAdministration, RegistrationAdministration>();
            services.AddSingleton<IRegistrationDocumentAdministration, RegistrationDocumentAdministration>();
            services.AddSingleton<IRegistrationIntegrationAdministration, RegistrationIntegrationAdministration>();
            services.AddSingleton<IRegistrationInvitationAdministration, RegistrationInvitationAdministration>();
            services.AddSingleton<IRegistrationOperationChecker, RegistrationOperationChecker>();
            services.AddSingleton<IRegistrationPaymentAdministration, RegistrationPaymentAdministration>();
            services.AddSingleton<IRegistrationPersonalDataAdministration, RegistrationPersonalDataAdministration>();
            services.AddSingleton<IRegistrationReviewAdministration, RegistrationReviewAdministration>();

            // app logic - convertors
            services.AddSingleton<IRegistrationInvitationConvertor, RegistrationInvitationConvertor>();
            services.AddSingleton<IRegistrationLastClassConvertor, RegistrationLastClassConvertor>();
            services.AddSingleton<IWebExRegistrationConvertor, WebExRegistrationConvertor>();

            // app logic - factories
            services.AddSingleton<IRegistrationPaymentFactory, RegistrationPaymentFactory>();

            // app logic - async request executors
            services.AddSingleton<IAsyncRequestExecutorFactory, RegistrationIntegrationAsyncExecutorFactory>();

            // system
            services.AddRegistrationLogicProviders();
            services.AddSingleton<IRegistrationStateProvider, RegistrationStateProvider>();
            
            services.AddSingleton<IRegistrationTypeResolver, RegistrationTypeResolver>();
            services.AddSingleton<IRegistrationEmailParameterResolverFactory, RegistrationEmailParameterResolverFactory>();
            services.AddSingleton<IEmailEntityParameterResolverFactoryForEntity, EmailEntityParameterResolverFactoryForRegistration>();
            services.AddSingleton<IEmailTemplateHierarchyResolverForEntity, EmailTemplateHierarchyResolverForRegistration>();
            services.AddSingleton<IRegistrationEmailService, RegistrationEmailService>();
            services.AddSingleton<IRegistrationPersonalDataService, RegistrationPersonalDataService>();
            services.AddSingleton<IRegistrationCommandService, RegistrationCommandService>();

            // system - email permission resolvers
            services.AddSingleton<IEmailPermissionResolverForEntity, EmailPermissionResolverForRegistration>();

            // system - job run executors
            services.AddSingleton<IJobRunExecutorFactory, RegistrationIntegrationSyncJobFactory>();

            // system - registration type feeders
            services.AddSingleton<IRegistrationTypeFeeder, RegistrationTypeFeeder>();
            services.AddSingleton<IRegistrationTypeFeederForClassCategory, RegistrationTypeFeederForClasses>();
            services.AddSingleton<IRegistrationTypeFeederForClassCategory, RegistrationTypeFeederForLectures>();
            services.AddSingleton<IRegistrationTypeFeederForClassCategory, RegistrationTypeFeederForDistanceClasses>();
            services.AddSingleton<IRegistrationTypeFeederForClassCategory, RegistrationTypeFeederForPrivateMaterialClasses>();

            // data
            services.AddSingleton<IRegistrationDatabaseLayer, RegistrationDatabaseLayer>();
            
            return services;
        }

        public static List<Profile> CreateProfiles()
        {
            return new List<Profile>
            {
                new RegistrationPaymentProfile()
            };
        }

        #region private methods
        private static void AddRegistrationLogicProviders(this IServiceCollection services)
        {
            // todo: refactor this object design
            services.AddSingleton<IRegistrationLogicProvider>(x =>
            {
                var result = new RegistrationLogicProvider(
                    x.GetService<IRegistrationDatabaseLayer>(),
                    x.GetService<IClassDatabaseLayer>(),
                    x.GetService<IEnumDatabaseLayer>(),
                    x.GetService<IRegistrationTypeFeeder>(),
                    x.GetService<IAddressConvertor>(),
                    x.GetService<IRegistrationStateProvider>(),
                    x.GetService<IRegistrationTypeResolver>());
                result.RegisterRegistrationConvertor(new RegistrationStudentConvertor(result, x.GetService<IClassOperationChecker>()));
                result.RegisterRegistrationConvertor(new RegistrationChildConvertor(result, x.GetService<IClassOperationChecker>()));
                result.RegisterRegistrationConvertor(new RegistrationWwaConvertor(result, x.GetService<IClassOperationChecker>()));
                return result;
            });

            services.AddSingleton<IRegistrationLogicProviderLocalised>(x =>
            {
                var result = new RegistrationLogicProvider(
                    x.GetService<IRegistrationDatabaseLayer>(),
                    x.GetService<IClassDatabaseLayer>(),
                    x.GetService<IEnumDatabaseLayer>(),
                    x.GetService<IRegistrationTypeFeeder>(),
                    x.GetService<IAddressConvertorLocalised>(),
                    x.GetService<IRegistrationStateProvider>(),
                    x.GetService<IRegistrationTypeResolver>(),
                    x.GetService<ILocalisationService>());
                result.RegisterRegistrationConvertor(new RegistrationStudentConvertor(result, x.GetService<IClassOperationChecker>()));
                result.RegisterRegistrationConvertor(new RegistrationChildConvertor(result, x.GetService<IClassOperationChecker>()));
                result.RegisterRegistrationConvertor(new RegistrationWwaConvertor(result, x.GetService<IClassOperationChecker>()));
                return result;
            });
        }
        #endregion
    }
}
