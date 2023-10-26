using AutomationSystem.Main.Core.Enums.Data;
using AutomationSystem.Main.Core.Enums.System;
using AutomationSystem.Shared.Contract.Enums.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.Enums
{
    public static class EnumServiceCollectionExtensions
    {
        public static IServiceCollection AddEnumServices(this IServiceCollection services)
        {
            // data
            services.AddSingleton<IEnumDataProvider, MainEnumDataProvider>();

            // system
            services.AddSingleton<IEnumMappingHelper, EnumMappingHelper>();

            return services;
        }
    }
}
