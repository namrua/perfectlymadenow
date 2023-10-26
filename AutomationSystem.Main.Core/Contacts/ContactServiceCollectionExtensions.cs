using AutoMapper;
using AutomationSystem.Main.Contract.Contacts.AppLogic;
using AutomationSystem.Main.Core.Contacts.AppLogic;
using AutomationSystem.Main.Core.Contacts.AppLogic.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Contacts.Data;
using AutomationSystem.Main.Core.Contacts.System.Emails;
using AutomationSystem.Shared.Contract.Emails.AppLogic;
using AutomationSystem.Shared.Contract.Emails.System;

namespace AutomationSystem.Main.Core.Contacts
{
    public static class ContactServiceCollectionExtensions
    {
        public static IServiceCollection AddContactServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IContactAdministration, ContactAdministration>();
            services.AddSingleton<IContactProvider, ContactProvider>();

            // data
            services.AddSingleton<IContactDatabaseLayer, ContactDatabaseLayer>();

            // system
            services.AddSingleton<IContactEmailService, ContactEmailService>();
            services.AddSingleton<IContactEmailParameterResolverFactory, ContactEmailParameterResolverFactory>();
            services.AddSingleton<IEmailPermissionResolverForEntity, EmailPermissionResolverForContact>();
            services.AddSingleton<IEmailTemplateHierarchyResolverForEntity, EmailTemplateHierarchyResolverForContactList>();

            return services;
        }

        public static List<Profile> CreateProfiles()
        {
            return new List<Profile>
            {
                new ContactProfile()
            };
        }
    }
}
