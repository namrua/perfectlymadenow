using System;
using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Main.Contract.PriceLists.AppLogic;
using AutomationSystem.Main.Core.PriceLists.AppLogic;
using AutomationSystem.Main.Core.PriceLists.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.PriceLists.Data;
using AutomationSystem.Main.Core.PriceLists.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.PriceLists
{
    public static class PriceListServiceCollectionExtensions
    {
        public static IServiceCollection AddPriceListServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IPriceListFactory, PriceListFactory>();
            services.AddSingleton<IPriceListService, PriceListService>();

            // data
            services.AddSingleton<IPriceListDatabaseLayer, PriceListDatabaseLayer>();

            // system
            services.AddSingleton<IPriceListTypeResolver, PriceListTypeResolver>();

            return services;
        }

        public static List<Profile> CreateProfiles(IServiceProvider sp)
        {
            return new List<Profile>
            {
                new PriceListProfile(sp.GetService<IPriceListTypeResolver>())
            };
        }
    }
}
