namespace AutomationSystem.Main.Core.Home.AppLogic.Models
{
    /// <summary>
    /// Encapsulates paypal shipping address
    /// </summary>
    public class PayloadShippingAddress
    {
        public string RecipientName { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }        
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }
}