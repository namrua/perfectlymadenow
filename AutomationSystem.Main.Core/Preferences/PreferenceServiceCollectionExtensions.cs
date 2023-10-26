using AutomationSystem.Main.Contract.Preferences.AppLogic;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Preferences.AppLogic;
using AutomationSystem.Main.Core.Preferences.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.Preferences.System;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.Preferences
{
    public static class PreferenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPreferenceServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IPreferenceAdministration, PreferenceAdministration>();

            // app logic - handler checkers
            services.AddTransient<IEventChecker<PersonDeletingEvent>, PersonToDeleteNotUsedInPreferences>();

            // system
            services.AddSingleton<IMainPreferenceProvider, MainPreferenceProvider>();

            return services;
        }
    }
}
