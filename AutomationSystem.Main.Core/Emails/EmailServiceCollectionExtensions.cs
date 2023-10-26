using AutomationSystem.Main.Core.Emails.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.Emails
{
    public static class EmailServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailServices(this IServiceCollection services)
        {
            // system
            // todo: move to share and make modular
            services.AddSingleton<IMainEmailHelper, MainEmailHelper>();
            services.AddSingleton<IEmailTypeResolver, EmailTypeResolver>();

            return services;
        }
    }
}
