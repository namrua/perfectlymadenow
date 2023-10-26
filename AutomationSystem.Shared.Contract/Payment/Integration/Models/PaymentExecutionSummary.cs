namespace AutomationSystem.Shared.Contract.Payment.Integration.Models
{
    /// <summary>
    /// Payment execution summary
    /// </summary>
    public class PaymentExecutionSummary
    {

        // public properties
        public string PaypalId { get; set; }
        public decimal Amount { get; set; }
        public decimal PaypalFee { get; set; }
        public string PaypalFeeText { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }

    }

}
