namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// PayPal transaction info 
    /// </summary>
    public class PayPalTransactionInfo
    {
        public string ClientToken { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Locale { get; set; }
        public string Environment { get; set; }
    }
}