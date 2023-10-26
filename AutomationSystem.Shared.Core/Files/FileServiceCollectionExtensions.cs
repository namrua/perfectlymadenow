using AutomationSystem.Shared.Contract.Files.System;
using AutomationSystem.Shared.Core.Files.Data;
using AutomationSystem.Shared.Core.Files.System;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.Files
{
    public static class FileServiceCollectionExtensions
    {
        public static IServiceCollection AddFileServices(this IServiceCollection services)
        {
            // data
            services.AddSingleton<IFileDatabaseLayer, FileDatabaseLayer>();

            // system
            services.AddSingleton<ICoreFileService, CoreFileService>();

            return services;
        }
    }
}