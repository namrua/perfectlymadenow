using System.Collections.Generic;
using AutoMapper;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Shared.Contract.Identities.AppLogic;
using AutomationSystem.Shared.Contract.Identities.Data;
using AutomationSystem.Shared.Contract.Identities.System;
using AutomationSystem.Shared.Core.Identities.AppLogic;
using AutomationSystem.Shared.Core.Identities.AppLogic.MappingProfiles;
using AutomationSystem.Shared.Core.Identities.Data;
using AutomationSystem.Shared.Core.Identities.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.Identities
{
    public static class IdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IIdentityAdministration, IdentityAdministration>();

            // data
            services.AddSingleton<IIdentityDatabaseLayer, IdentityDatabaseLayer>();

            // system
            services.AddSingleton<IClaimsIdentityBuilder, ClaimsIdentityBuilder>();
            services.AddSingleton<IIdentityProvider, IdentityProvider>();
            services.AddSingleton<IIdentityResolver, IdentityResolver>();

            return services;
        }

        public static List<Profile> CreateProfiles()
        {
            return new List<Profile>
            {
                new UserProfile()
            };
        }
    }
}   