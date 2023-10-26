using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.AppLogic;
using AutomationSystem.Main.Core.Persons.AppLogic.Convertors;
using AutomationSystem.Main.Core.Persons.AppLogic.EventCheckers;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.Persons
{
    public static class PersonServiceCollectionExtensions
    {
        public static IServiceCollection AddPersonServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IPersonAdministration, PersonAdministration>();

            // app logic - convertors
            services.AddSingleton<IPersonConvertor, PersonConvertor>();

            // app logic - event checkers
            services.AddTransient<IEventChecker<ProfileDeletingEvent>, ProfileToDeleteHasNoPersonEventChecker>();

            // data
            services.AddSingleton<IPersonDatabaseLayer, PersonDatabaseLayer>();

            return services;
        }
    }
}
