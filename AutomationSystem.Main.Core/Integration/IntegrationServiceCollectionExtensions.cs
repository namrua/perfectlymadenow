using AutomationSystem.Base.Contract.Integration;
using AutomationSystem.Main.Contract.Integration.AppLogic;
using AutomationSystem.Main.Core.Integration.AppLogic;
using AutomationSystem.Main.Core.Integration.AppLogic.Convertors;
using AutomationSystem.Main.Core.Integration.System;
using AutomationSystem.Main.Core.Integration.System.NoIntegration;
using AutomationSystem.Main.Core.Integration.System.WebEx;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.WebEx.Authentication;

namespace AutomationSystem.Main.Core.Integration
{
    public static class IntegrationServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IMainAccountAdministration, MainAccountAdministration>();
            services.AddSingleton<IMainProgramAdministration, MainProgramAdministration>();

            // app logic - convertors
            services.AddSingleton<IMainAccountConvertor, MainAccountConvertor>();
            services.AddSingleton<IMainProgramConvertor, MainProgramConvertor>();

            // system
            services.AddSingleton<IGenericIntegrationDataProvider, GenericIntegrationDataProvider>();

            // system - no integration
            services.AddSingleton<IEntityIntegrationProvider, NoIntegrationEntityIntegrationProvider>();
            services.AddSingleton<IIntegrationDataProvider, NoIntegrationDataProvider>();
            services.AddSingleton<IRegistrationIntegrationRequestHandlerFactory, NoIntegrationRequestHandlerFactory>();
            services.AddSingleton<IRegistrationIntegrationSyncExecutorFactory, NoIntegrationSyncExecutorFactory>();

            // system - WebEx
            services.AddSingleton<IIntegrationDataProvider, WebExIntegrationDataProvider>();
            services.AddSingleton<IRegistrationIntegrationRequestHandlerFactory, WebExRegistrationIntegrationRequestHandlerFactory>();
            services.AddSingleton<IRegistrationIntegrationSyncExecutorFactory, WebExRegistrationIntegrationSyncExecutorFactory>();

            return services;
        }
    }
}
