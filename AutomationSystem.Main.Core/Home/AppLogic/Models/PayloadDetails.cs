namespace AutomationSystem.Main.Core.Home.AppLogic.Models
{
    /// <summary>
    /// Encapsulates paypal detail
    /// </summary>
    public class PayloadDetails
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayerId { get; set; }
        public string CountryCode { get; set; }

        public PayloadShippingAddress ShippingAddress { get; set; }
    }
}