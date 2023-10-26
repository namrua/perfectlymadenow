using AutoMapper;
using AutomationSystem.Shared.Contract.BatchUploads.Data;
using AutomationSystem.Shared.Contract.BatchUploads.System;
using AutomationSystem.Shared.Core.BatchUploads.AppLogic.MappingProfiles;
using AutomationSystem.Shared.Core.BatchUploads.Data;
using AutomationSystem.Shared.Core.BatchUploads.System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace AutomationSystem.Shared.Core.BatchUploads
{
    public static class BatchUploadServiceCollectionExtensions
    {
        public static IServiceCollection AddBatchUploadServices(this IServiceCollection services)
        {
            // data
            services.AddSingleton<IBatchUploadDatabaseLayer, BatchUploadDatabaseLayer>();

            // system
            services.AddSingleton<IBatchUploadFactory, BatchUploadFactory>();
            services.AddSingleton<IBatchUploadService, BatchUploadService>();
            services.AddSingleton<IBatchUploadValidationHelper, BatchUploadValidationHelper>();

            return services;
        }

        public static List<Profile> CreateProfiles()
        {
            return new List<Profile>
            {
                new BatchUploadProfile()
            };
        }
    }
}