using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments
{
    /// <summary>
    /// Paypal record detail
    /// </summary>
    public class PayPalRecordDetail 
    {

        [DisplayName("PayPal ID")]
        public string PayPalId { get; set; }

        [DisplayName("Total")]
        public decimal Total { get; set; }

        [DisplayName("Fee")]
        public decimal Fee { get; set; }

        [DisplayName("Payer ID")]
        public string PayerId { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("CountryCode")]
        public string CountryCode { get; set; }

        [DisplayName("Shipping address")]
        public PayloadAddressDetail ShippingAddress { get; set; }

        [DisplayName("Transaction number")]
        public string TransactionId { get; set; }


        [DisplayName("Name")] public string FullName { get; set; }

        // constructor
        public PayPalRecordDetail()
        {
            ShippingAddress = new PayloadAddressDetail();
        }

    }
}
