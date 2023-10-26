using AutomationSystem.Base.Contract.Identities.Models.Events;
using AutomationSystem.Shared.Contract.Payment.AppLogic;
using AutomationSystem.Shared.Contract.Payment.Data;
using AutomationSystem.Shared.Contract.Payment.Integration;
using AutomationSystem.Shared.Core.Payment.AppLogic;
using AutomationSystem.Shared.Core.Payment.AppLogic.EventCheckers;
using AutomationSystem.Shared.Core.Payment.Data;
using AutomationSystem.Shared.Core.Payment.Integration;
using Microsoft.Extensions.DependencyInjection;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Shared.Core.Payment
{
    public static class PaymentServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentServices(this IServiceCollection services)
        {
            // app logic
            services.AddSingleton<IPaymentAdministration, PaymentAdministration>();

            // app logic - convertors
            services.AddSingleton<IPayPalKeyConvertor, PayPalKeyConvertor>();

            // app logic - event checkers
            services.AddTransient<IEventChecker<UserGroupDeletingEvent>, UserGroupToDeleteHasNoPayPalKeyEventChecker>();

            // data
            services.AddSingleton<IPaymentDatabaseLayer, PaymentDatabaseLayer>();

            // integration
            services.AddSingleton<IPayPalBraintreeProviderFactory, PayPalBraintreeProviderFactory>();

            return services;
        }
    }
}