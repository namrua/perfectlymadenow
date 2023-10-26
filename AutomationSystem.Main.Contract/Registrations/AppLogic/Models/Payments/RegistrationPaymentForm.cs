using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments
{
    /// <summary>
    /// Registration payment form
    /// </summary>
    public class RegistrationPaymentForm    
    {

        [HiddenInput]
        public long ClassRegistrationId { get; set; }

        [MaxLength(32)]
        [DisplayName("Check number")]
        public string CheckNumber { get; set; }

        [MaxLength(32)]
        [DisplayName("Transaction number")]
        public string TransactionNumber { get; set; }

        [DisplayName("PayPal fee")]
        [Range(0, 1000000)]
        public decimal? PayPalFee { get; set; }

        [DisplayName("Total PayPal")]
        [Range(0, 1000000)]
        public decimal? TotalPayPal { get; set; }

        [DisplayName("Total check")]
        [Range(0, 1000000)]
        public decimal? TotalCheck { get; set; }

        [DisplayName("Total cash")]
        [Range(0, 1000000)]
        public decimal? TotalCash { get; set; }

        [DisplayName("Total credit card")]
        [Range(0, 1000000)]
        public decimal? TotalCreditCard { get; set; }

        [DisplayName("Is paid by PMI")]
        public bool IsPaidPmi { get; set; }

        [DisplayName("Other coordinator")]
        public bool IsAbsentee { get; set; }

    }
}
