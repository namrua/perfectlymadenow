using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Main.Contract.DistanceProfiles.AppLogic;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.DistanceProfiles.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using AutomationSystem.Main.Core.PriceLists.AppLogic.Models.Events;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using AutomationSystem.Shared.Contract.Payment.AppLogic.Models.Events;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.DistanceProfiles
{
    public static class DistanceProfileServiceCollectionExtensions
    {
        public static IServiceCollection AddDistanceProfileServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IDistanceProfileAdministration, DistanceProfileAdministration>();

            // app logic - event checkers
            services.AddTransient<IEventChecker<PayPalAccountDeletingEvent>, PayPalAccountToDeleteHasNoDistanceProfileEventChecker>();
            services.AddTransient<IEventChecker<PersonDeletingEvent>, PersonToDeleteHasNoDistanceProfileEventChecker>();
            services.AddTransient<IEventChecker<PriceListDeletingEvent>, PriceListToDeleteHasNoDistanceProfileEventChecker>();
            services.AddTransient<IEventChecker<ProfileDeletingEvent>, ProfileToDeleteHasNoDistanceProfileEventChecker>();

            // data
            services.AddSingleton<IDistanceProfileDatabaseLayer, DistanceProfileDatabaseLayer>();
            services.AddSingleton<IDistanceProfileFactory, DistanceProfileFactory>();

            return services;
        }

        public static List<Profile> CreateProfiles()
        {
            return new List<Profile>
            {
                new DistanceProfileProfile()
            };
        }
    }
}
