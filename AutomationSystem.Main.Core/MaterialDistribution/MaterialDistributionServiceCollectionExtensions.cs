using AutomationSystem.Main.Contract.MaterialDistribution.AppLogic;
using AutomationSystem.Main.Core.Classes.AppLogic.Models.Events;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Convertors;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.EventHandlers;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.Handlers;
using AutomationSystem.Main.Core.MaterialDistribution.AppLogic.MaterialRecipientIntegrations;
using AutomationSystem.Main.Core.MaterialDistribution.Data;
using AutomationSystem.Main.Core.MaterialDistribution.System;
using AutomationSystem.Main.Core.MaterialDistribution.System.Emails;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.MaterialDistribution
{
    public static class MaterialDistributionServiceCollectionExtensions
    {
        public static IServiceCollection AddMaterialDistributionServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IClassMaterialAdministration, ClassMaterialAdministration>();
            services.AddSingleton<IClassMaterialBusinessRules, ClassMaterialBusinessRules>();
            services.AddSingleton<IClassMaterialOperationChecker, ClassMaterialOperationChecker>();
            services.AddSingleton<IClassMaterialService, ClassMaterialService>();
            services.AddSingleton<IMaterialAvailabilityResolver, MaterialAvailabilityResolver>();
            services.AddSingleton<IMaterialPasswordGenerator, MaterialPasswordGenerator>();
            services.AddSingleton<IMaterialRecipientIntegrationProvider, MaterialRecipientIntegrationProvider>();
            services.AddSingleton<IPdfEncryptor, PdfEncryptor>();

            // app logic - convertors
            services.AddSingleton<IClassMaterialConvertor, ClassMaterialConvertor>();
            services.AddSingleton<IClassMaterialFileConvertor, ClassMaterialFileConvertor>();
            services.AddSingleton<IClassMaterialMonitoringConvertor, ClassMaterialMonitoringConvertor>();
            services.AddSingleton<IClassMaterialRecipientConvertor, ClassMaterialRecipientConvertor>();

            // app logic - event handlers
            services.AddTransient<IEventHandler<ClassCreatedEvent>, CreateClassMaterialsEventHandler>();
            services.AddTransient<IEventHandler<ClassPersonsChangedEvent>, SyncMaterialsForPersonsEventHandler>();

            // app logic - handlers
            services.AddSingleton<IClassMaterialDistributionHandler, ClassMaterialDistributionHandler>();

            // app logic - material recipient integrations
            services.AddSingleton<IMaterialRecipientIntegration, PersonMaterialIntegration>();
            services.AddSingleton<IMaterialRecipientIntegration, RegistrationMaterialIntegration>();

            // data
            services.AddSingleton<IClassMaterialDatabaseLayer, ClassMaterialDatabaseLayer>();

            // system
            services.AddSingleton<IMaterialEmailService, MaterialEmailService>();
            services.AddSingleton<IMaterialEmailParameterResolverFactory, MaterialEmailParameterResolverFactory>();

            return services;
        }
    }
}
