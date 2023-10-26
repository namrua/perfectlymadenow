
using AutomationSystem.Shared.Contract.Payment.Integration.Models;

namespace AutomationSystem.Shared.Contract.Payment.Integration
{
    /// <summary>
    /// PayPal braintree provider
    /// </summary>
    public interface IPayPalBraintreeProvider
    {

        // generates client token
        string GenerateClientToken();

        // execute payment
        PaymentExecutionSummary ExecutePayment(string nonce, PaymentListItemInfo listItemInfo);

    }

}
