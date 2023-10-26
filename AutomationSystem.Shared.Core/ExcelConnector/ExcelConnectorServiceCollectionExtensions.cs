using AutomationSystem.Shared.Contract.ExcelConnector.Integration;
using AutomationSystem.Shared.Core.ExcelConnector.Integration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomationSystem.Shared.Core.ExcelConnector
{
    public static class ExcelConnectorServiceCollectionExtensions
    {
        public static IServiceCollection AddExcelConnectorServices(this IServiceCollection services)
        {
            // integration
            services.AddSingleton<IExcelConnectorFactory, ExcelConnectorFactory>();

            return services;
        }
    }
}