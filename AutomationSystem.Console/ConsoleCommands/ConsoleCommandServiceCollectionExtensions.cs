using AutomationSystem.Console.ConsoleCommands.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Console.ConsoleCommands
{
    public static class ConsoleCommandServiceCollectionExtensions
    {
        public static IServiceCollection AddConsoleCommands(this IServiceCollection services)
        {
            services.AddTransient<IConsoleCommand<EmailMigrationParameters>, EmailMigrationCommand>();
            services.AddTransient<IConsoleCommand<SendingEmailParameters>, SendingEmailCommand>();

            return services;
        }
    }
}
