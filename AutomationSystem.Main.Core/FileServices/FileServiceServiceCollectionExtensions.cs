using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.FileServices.System.Convertors;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.FileServices
{
    public static class FileServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddFileServiceServices(this IServiceCollection services)
        {
            // system
            services.AddSingleton<IMainFileService, MainFileService>();

            // system - convertors
            services.AddSingleton<IMainFileConvertor, MainFileConvertor>();

            return services;
        }
    }
}
