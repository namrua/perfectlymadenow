using AutomationSystem.Base.Contract.Identities.Models.Events;
using AutomationSystem.Base.Contract.Integration;
using AutomationSystem.Shared.Core.ConferenceAccounts.AppLogic.EventCheckers;
using AutomationSystem.Shared.Core.ConferenceAccounts.Data;
using AutomationSystem.Shared.Core.ConferenceAccounts.System;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Shared.Core.ConferenceAccounts
{   
    public static class ConferenceAccountServiceCollectionExtensions
    {
        public static IServiceCollection AddConferenceAccountServices(this IServiceCollection services)
        {
            // app logic - event checkers
            services.AddTransient<IEventChecker<UserGroupDeletingEvent>, UserGroupToDeleteHasNoConferenceAccountEventChecker>();

            // data
            services.AddSingleton<IConferenceAccountDatabaseLayer, ConferenceAccountDatabaseLayer>();
            
            // system
            services.AddSingleton<IConferenceAccountService, ConferenceAccountService>();

            return services;
        }
    }
}