namespace AutomationSystem.Shared.Contract.Payment.Integration
{
    public interface IPayPalBraintreeProviderFactory
    {
        IPayPalBraintreeProvider CreatePayPalBraintreeProvider(string braintreeGatewayToken);
    }
}
