using AutomationSystem.Base.Contract.Integration;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.WebEx.Contract.Programs;
using PerfectlyMadeInc.WebEx.Programs.Data;

namespace PerfectlyMadeInc.WebEx.Programs
{
    public static class ProgramServiceCollectionExtensions
    {
        public static IServiceCollection AddProgramServices(this IServiceCollection services)
        {
            services.AddSingleton<IEntityIntegrationProvider, ProgramEntityIntegrationProvider>();
            services.AddSingleton<IEventDataProvider, EventDataProvider>();
            services.AddSingleton<IProgramAdministration, ProgramAdministration>();
            services.AddSingleton<IProgramDatabaseLayer, ProgramDatabaseLayer>();
            return services;
        }
    }
}