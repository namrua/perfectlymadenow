using System.Collections.Generic;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic;
using AutomationSystem.Main.Core.FormerClasses.AppLogic;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.BatchDataFetchers;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.Convertors;
using AutomationSystem.Main.Core.FormerClasses.AppLogic.MappingProfiles;
using AutomationSystem.Main.Core.FormerClasses.Data;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.BatchUploads.AppLogic;
using Microsoft.Extensions.DependencyInjection;
using Profile = AutoMapper.Profile;

namespace AutomationSystem.Main.Core.FormerClasses
{
    public static class FormerClassesServiceCollectionExtensions
    {
        public static IServiceCollection AddFormerClassesServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IBatchUploadMergeResolver<FormerStudent>, FormerStudentBatchMergeResolver>();
            services.AddSingleton<IFormerStudentUploadValidator, FormerStudentBatchValidator>();
            services.AddSingleton<IFormerClassAdministration, FormerClassAdministration>();
            services.AddSingleton<IFormerClassFactory, FormerClassFactory>();
            services.AddSingleton<IFormerClassPropagator, FormerClassPropagator>();
            services.AddSingleton<IFormerStudentAdministration, FormerStudentAdministration>();
            services.AddSingleton<IFormerStudentBatchUploadAdministration, FormerStudentBatchUploadAdministration>();
            services.AddSingleton<IFormerStudentBatchUploadFactory, FormerStudentBatchUploadFactory>();
            services.AddSingleton<IFormerStudentBatchUploadValueResolver, FormerStudentBatchUploadValueResolver>();

            // app logic - batch upload fetcher
            services.AddSingleton<IFormerStudentBatchFileDataFetcher, ExcelFormerStudentsFetcher>();
            services.AddSingleton<IFormerStudentBatchFileDataFetcher, CsvFormerStudentsFetcher>();

            // app logic - convertors
            services.AddSingleton<IClassFormerClassConvertor, ClassFormerClassConvertor>();
            services.AddSingleton<IFormerStudentBatchConvertor, FormerStudentBatchConvertor>();
            services.AddSingleton<IFormerStudentConvertor, FormerStudentConvertor>();

            // data
            services.AddSingleton<IFormerDatabaseLayer, FormerDatabaseLayer>();

            return services;
        }
        public static List<Profile> CreateProfiles()
        {
            return new List<Profile>
            {
                new FormerClassProfile()
            };
        }
    }
}
