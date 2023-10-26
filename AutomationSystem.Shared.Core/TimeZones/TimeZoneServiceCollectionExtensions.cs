using AutomationSystem.Shared.Contract.TimeZones.System;
using AutomationSystem.Shared.Core.TimeZones.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.TimeZones
{
    public static class TimeZoneServiceCollectionExtensions
    {
        public static IServiceCollection AddTimeZoneServices(this IServiceCollection services)
        {
            // system
            services.AddSingleton<ITimeZoneService, TimeZoneService>();

            return services;
        }
    }
}