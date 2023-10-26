using AutomationSystem.Main.Contract.RegistrationUpload.AppLogic;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic;
using AutomationSystem.Main.Core.RegistrationUpload.AppLogic.BatchDataFetchers;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.RegistrationUpload
{
    public static class RegistrationUploadServiceCollectionExtensions
    {
        public static IServiceCollection AddRegistrationUploadServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IBatchUploadMergeResolver<ClassRegistration>, StudentRegistrationBatchMergeResolver>();
            services.AddSingleton<IRegistrationBatchUploadAdministration, RegistrationBatchUploadAdministration>();
            services.AddSingleton<IRegistrationBatchUploadFactory, RegistrationBatchUploadFactory>();
            services.AddSingleton<IStudentRegistrationBatchMapper, StudentRegistrationBatchMapper>();
            services.AddSingleton<IStudentRegistrationBatchValidator, StudentRegistrationBatchValidator>();
            services.AddSingleton<IStudentRegistrationBatchUploadValueResolver, StudentRegistrationBatchUploadValueResolver>();

            // app logic - batch data fetcher
            services.AddSingleton<IStudentRegistrationBatchDataFileFetcher, ExcelStudentRegistrationFetcher>();
            
            
            return services;
        }
    }
}
