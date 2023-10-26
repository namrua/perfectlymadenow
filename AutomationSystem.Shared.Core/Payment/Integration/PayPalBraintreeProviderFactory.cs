using AutomationSystem.Shared.Contract.Payment.Integration;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Payment.Integration
{
    public class PayPalBraintreeProviderFactory : IPayPalBraintreeProviderFactory
    {
        private readonly ITracerFactory tracerFactory;

        public PayPalBraintreeProviderFactory(ITracerFactory tracerFactory)
        {
            this.tracerFactory = tracerFactory;
        }

        public IPayPalBraintreeProvider CreatePayPalBraintreeProvider(string braintreeGatewayToken)
        {
            return new PayPalBraintreeProvider(braintreeGatewayToken, tracerFactory);
        }
    }
}
