using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Shared.Contract.Localisation.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.Addresses.AppLogic
{
    public static class AddressServiceCollectionExtensions
    {
        public static IServiceCollection AddAddressServices(this IServiceCollection services)
        {
            // app logic - convertors
            services.AddSingleton<IAddressConvertor>(x => new AddressConvertor(null));
            services.AddSingleton<IAddressConvertorLocalised>(x => new AddressConvertor(x.GetService<ILocalisationService>()));

            return services;
        }
        
    }
}
