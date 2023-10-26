using System;
using System.Globalization;
using AutomationSystem.Shared.Contract.Payment.Integration;
using AutomationSystem.Shared.Contract.Payment.Integration.Models;
using Braintree;
using PerfectlyMadeInc.DesignTools.Contract.Diagnostics;

namespace AutomationSystem.Shared.Core.Payment.Integration
{
    /// <summary>
    /// PayPal braintree provider
    /// </summary>
    public class PayPalBraintreeProvider : IPayPalBraintreeProvider
    {

        // private properties        
        private readonly string gatewayToken;

        // private components
        private readonly ITracer tracer;

        // constructor
        public PayPalBraintreeProvider(string braintreeGatewayToken, ITracerFactory tracerFactory)
        {
            gatewayToken = braintreeGatewayToken ?? throw new ArgumentNullException(nameof(braintreeGatewayToken));
            tracer = tracerFactory.CreateTracer<PayPalBraintreeProvider>();
        }

        // generates client token
        public string GenerateClientToken()
        {
            var gateway = new BraintreeGateway(gatewayToken);
            var clientToken = gateway.ClientToken.Generate();
            return clientToken;
        }

        // execute payment
        public PaymentExecutionSummary ExecutePayment(string nonce, PaymentListItemInfo listItemInfo)
        {
            var gateway = new BraintreeGateway(gatewayToken);
            if (listItemInfo.ItemName.Length > 27)
                listItemInfo.ItemName = listItemInfo.ItemName.Substring(0, 27);
            var request = new TransactionRequest
            {
                Amount = listItemInfo.Price,
                PaymentMethodNonce = nonce,
                LineItems = new[]
                {
                    new TransactionLineItemRequest()
                    {
                        Quantity = 1,
                        LineItemKind = TransactionLineItemKind.DEBIT,
                        Name = listItemInfo.ItemName,
                        UnitAmount = listItemInfo.Price,
                        TotalAmount = listItemInfo.Price
                    }
                },
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                    PayPal = new TransactionOptionsPayPalRequest()
                    {
                    CustomField = listItemInfo.PayPalId
                }
                }
            };

            var result = new PaymentExecutionSummary();
            Result<Transaction> payPalResult = gateway.Transaction.Sale(request);
            result.IsSuccess = payPalResult.IsSuccess();
            result.Message = payPalResult.Message;
            if (result.IsSuccess)
            {
                var settledTransaction = payPalResult.Target;
                if (settledTransaction != null)
                {
                    result.PaypalId = settledTransaction.Id;
                    result.Amount = settledTransaction.Amount ?? 0;
                    result.PaypalFeeText = settledTransaction.PayPalDetails?.TransactionFeeAmount;
                    result.TransactionId = settledTransaction.PayPalDetails?.CaptureId;
                    if (result.PaypalFeeText != null && TryParseFee(result.PaypalFeeText, out var payPalFee))
                        result.PaypalFee = payPalFee;
                }
            }
            else
            {
                // logs error messages
                tracer.Error($"PayPal error. Message: {result.Message}\nErrors: {string.Join("; ", payPalResult.Errors.All())}");
            }
            return result;
        }

        #region private methods

        private static bool TryParseFee(string payPalFeeText, out decimal payPalFee)
        {
            payPalFeeText = payPalFeeText.Replace(" ", "");
            var lastDot = payPalFeeText.LastIndexOf('.');
            var lastComma = payPalFeeText.LastIndexOf(',');

            if (lastComma >= 0)
            {
                if (lastComma > lastDot)
                {
                    var beforeLastComma = payPalFeeText
                        .Substring(0, lastComma)
                        .Replace(",", "")
                        .Replace(".", "");

                    payPalFeeText = beforeLastComma + "." + payPalFeeText.Substring(lastComma + 1);
                }
            }

            return decimal.TryParse(payPalFeeText, NumberStyles.Any, CultureInfo.InvariantCulture, out payPalFee);
        }

        #endregion
    }
}
