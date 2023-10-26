namespace AutomationSystem.Shared.Contract.Payment.Integration.Models
{
    /// <summary>
    /// Encapsulates payment list item informations
    /// </summary>
    public class PaymentListItemInfo
    {

        public string PayPalId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }

    }

}
