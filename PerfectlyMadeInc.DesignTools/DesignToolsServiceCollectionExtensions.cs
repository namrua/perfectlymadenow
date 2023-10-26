using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;
using PerfectlyMadeInc.DesignTools.Contract.Events;
using PerfectlyMadeInc.DesignTools.Diagnostics;
using PerfectlyMadeInc.DesignTools.Events;

namespace PerfectlyMadeInc.DesignTools
{
    public static class DesignToolsServiceCollectionExtensions
    {
        public static IServiceCollection AddDesignToolsServices(this IServiceCollection services)
        {
            services.AddSingleton<ITracerFactory, ComponentTracerFactory>();
            services.AddSingleton<IEventDispatcher, EventDispatcher>();
            return services;
        }
    }
}