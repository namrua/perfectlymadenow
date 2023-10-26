using AutoMapper;
using AutomationSystem.Main.Contract.Profiles.AppLogic;
using AutomationSystem.Main.Core.Profiles.AppLogic;
using AutomationSystem.Main.Core.Profiles.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.System;
using AutomationSystem.Main.Core.Profiles.System.Emails;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.System;
using AutomationSystem.Shared.Contract.Identities.System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace AutomationSystem.Main.Core.Profiles
{
    public static class ProfileServiceCollectionExtensions
    {
        public static IServiceCollection AddProfileServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IProfileAdministration, ProfileAdministration>();
            services.AddSingleton<IProfileEmailAdministration, ProfileEmailAdministration>();
            services.AddSingleton<IProfileUsersAdministration, ProfileUsersAdministration>();

            // data
            services.AddSingleton<IProfileDatabaseLayer, ProfileDatabaseLayer>();

            // system
            services.AddSingleton<IEmailTemplateHierarchyResolverForEntity, EmailTemplateHierarchyResolverForProfile>();
            services.AddSingleton<IUserGroupMembershipProvider, ProfileMembershipProvider>();

            // system - email permission resolvers
            services.AddSingleton<IEmailPermissionResolverForEntity, EmailPermissionResolverForProfile>();

            return services;
        }

        public static List<Profile> CreateProfiles()
        {
            return new List<Profile>
            {
                new ProfileProfile()
            };
        }
    }
}
