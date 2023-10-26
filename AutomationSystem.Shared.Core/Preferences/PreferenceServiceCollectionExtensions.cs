using AutomationSystem.Shared.Contract.Preferences.Data;
using AutomationSystem.Shared.Contract.Preferences.System;
using AutomationSystem.Shared.Core.Preferences.Data;
using AutomationSystem.Shared.Core.Preferences.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.Preferences
{
    public static class PreferenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPreferenceServices(this IServiceCollection services)
        {
            // data
            services.AddSingleton<IPreferenceDatabaseLayer>(x => new PreferenceDatabaseLayerCache(new PreferenceDatabaseLayer()));

            // system
            services.AddSingleton<ICorePreferenceProvider, CorePreferenceProvider>();

            return services;
        }
    }
}