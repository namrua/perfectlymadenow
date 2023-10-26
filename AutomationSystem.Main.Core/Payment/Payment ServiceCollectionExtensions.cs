using AutomationSystem.Main.Contract.Payment.AppLogic;
using Microsoft.Extensions.DependencyInjection;
using AutomationSystem.Main.Core.Payment.AppLogic;
using AutomationSystem.Main.Core.Payment.AppLogic.Convertors;

namespace AutomationSystem.Main.Core.Payment
{
    public static class Payment_ServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IMainPaymentAdministration, MainPaymentAdmninistration>();

            // app logic - convertors
            services.AddSingleton<IMainPayPalKeyConvertor, MainPayPalKeyConvertor>();

            return services;
        }
    }
}
