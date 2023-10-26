using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.WwaCrf
{
    /// <summary>
    /// WWWA report persons info
    /// </summary>
    public class WwaCrfPersonsInfo
    {
        // Registrant properties
        [SheetField("LastNameRegistrant")]
        public string LastNameRegistrant { get; set; }

        [SheetField("FirstNameRegistrant")]
        public string FirstNameRegistrant { get; set; }

        [SheetField("CompleteAddressRegistrant")]
        public string CompleteAddressRegistrant { get; set; }

        [SheetField("Email")]
        public string Email { get; set; }

        [SheetField("CheckNumber")]
        public string CheckNumber { get; set; }

        [SheetField("TransactionNumber")]
        public string TransactionNumber { get; set; }

        [SheetField("PayPalFee")]
        public decimal? PayPalFee { get; set; }

        [SheetField("TotalPayPal")]
        public decimal? TotalPayPal { get; set; }

        [SheetField("TotalCheck")]
        public decimal? TotalCheck { get; set; }

        [SheetField("TotalCash")]
        public decimal? TotalCash { get; set; }

        [SheetField("TotalCreditCard")]
        public decimal? TotalCreditCard { get; set; }

        [SheetField("NetPayPal")]
        public decimal? NetPayPal { get; set; }

        [SheetField("TotalRevenue")]
        public decimal? TotalRevenue { get; set; }

        // Participant properties
        [SheetField("LastNameParticipant")]
        public string LastNameParticipant { get; set; }

        [SheetField("FirstNameParticipant")]
        public string FirstNameParticipant { get; set; }

        [SheetField("CompleteAddressParticipant")]
        public string CompleteAddressParticipant { get; set; }

        [SheetField("CountryParticipant")]
        public string CountryParticipant { get; set; }
    }
}
