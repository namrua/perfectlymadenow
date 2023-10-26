using System.Linq;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Core.Enums.Data;
using AutomationSystem.Shared.Core.Enums.Data.EnumDataProviders;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.Enums
{
    public static class EnumServiceCollectionExtensions
    {
        public static IServiceCollection AddEnumServices(this IServiceCollection services)
        {
            // data
            services.AddSingleton<IEnumDatabaseLayer>(x =>
            {
                var enumDataProviders = x.GetServices<IEnumDataProvider>().ToList();
                return new EnumDatabaseCache(new EnumDatabaseLayer(enumDataProviders), enumDataProviders);
            });

            // data - enum data providers
            services.AddSingleton<IEnumDataProvider, CoreEnumDataProvider>();

            return services;
        }
    }
}