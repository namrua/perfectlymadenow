using AutomationSystem.Main.Contract.Reports.AppLogic;
using AutomationSystem.Main.Core.Reports.AppLogic;
using AutomationSystem.Main.Core.Reports.AppLogic.Convertors;
using AutomationSystem.Main.Core.Reports.Data;
using AutomationSystem.Main.Core.Reports.System;
using AutomationSystem.Main.Core.Reports.System.DataProviders;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Main.Core.Reports
{
    public static class ReportingServiceCollectionExtensions
    {
        public static IServiceCollection AddReportingServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IGeneralReportService, GeneralReportService>();

            // app logic - convertors
            services.AddSingleton<IGeneralReportServiceConvertor, GeneralReportServiceConvertor>();

            // data
            services.AddSingleton<IFinancialFormDatabaseLayer, FinancialFormDatabaseLayer>();

            // system
            services.AddSingleton<IClassReportFactory, ClassReportFactory>();
            services.AddSingleton<IDistanceReportService, DistanceReportService>();
            services.AddSingleton<IReportService, ReportService>();

            // system - data providers
            services.AddSingleton<IDistanceReportDataProviderFactory, DistanceReportDataProviderFactory>();

            return services;
        }
    }
}
