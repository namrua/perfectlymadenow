using AutomationSystem.Shared.Contract.WordWorkflows.Integration;
using AutomationSystem.Shared.Core.WordWorkflows.Integration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.WordWorkflows
{
    public static class WordWorkflowServiceCollectionExtensions
    {
        public static IServiceCollection AddWordWorkflowServices(this IServiceCollection services)
        {
            // integration
            services.AddSingleton<IWordWorkflowFactory, WordWorkflowFactory>();
            
            return services;
        }
    }
}   