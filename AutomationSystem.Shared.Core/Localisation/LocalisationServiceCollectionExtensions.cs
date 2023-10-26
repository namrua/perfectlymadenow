using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using AutomationSystem.Shared.Contract.Localisation.System;
using AutomationSystem.Shared.Core.Localisation.AppLogic;
using AutomationSystem.Shared.Core.Localisation.Data;
using AutomationSystem.Shared.Core.Localisation.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.Localisation
{
    public static class LocalisationServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalisationServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<ILocalisationAdministration, LocalisationAdministration>();

            // data
            services.AddSingleton<ILocalisationDatabaseLayer>(x => new LocalisationDatabaseLayerCache(new LocalisationDatabaseLayer()));

            // system
            services.AddSingleton<ILanguagePersistor, LanguagePersistor>();
            services.AddSingleton<ILocalisationService, LocalisationService>();

            return services;
        }
    }
}