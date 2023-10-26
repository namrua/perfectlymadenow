using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments
{
    /// <summary>
    /// Registration payment detail
    /// </summary>
    public class RegistrationPaymentDetail
    {
           
        [DisplayName("ID")]
        public long ClassRegistrationPaymentId { get; set; }     
      
        [DisplayName("Check number")]
        public string CheckNumber { get; set; }
       
        [DisplayName("Transaction number")]
        public string TransactionNumber { get; set; }

        [DisplayName("PayPal fee")]
        public decimal? PayPalFee { get; set; }

        [DisplayName("Total PayPal")]
        public decimal? TotalPayPal { get; set; }

        [DisplayName("Total check")]
        public decimal? TotalCheck { get; set; }

        [DisplayName("Total cash")]
        public decimal? TotalCash { get; set; }

        [DisplayName("Total credit card")]
        public decimal? TotalCreditCard { get; set; }

        [DisplayName("Is paid by PMI")]
        public bool IsPaidPmi { get; set; }

        [DisplayName("Other coordinator")]
        public bool IsAbsentee { get; set; }

    }
}
