using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.Crf
{
    /// <summary>
    /// Attendee info
    /// </summary>
    public class CrfPersonInfo
    {
        [SheetField("FirstName")]
        public string FirstName { get; set; }

        [SheetField("LastName")]
        public string LastName { get; set; }

        [SheetField("AddressLine1")]
        public string AddressLine1 { get; set; }

        [SheetField("AddressLine2")]
        public string AddressLine2 { get; set; }

        [SheetField("City")]
        public string City { get; set; }

        [SheetField("State")]
        public string State { get; set; }

        [SheetField("Zip")]
        public string Zip { get; set; }

        [SheetField("Country")]
        public string Country { get; set; }

        [SheetField("Phone")]
        public string Phone { get; set; }

        [SheetField("Email")]
        public string Email { get; set; }

        [SheetField("IsTransLanguage")]
        public string IsTransLanguage { get; set; }

        [SheetField("StatCode")]
        public string StatCode { get; set; }

        [SheetField("Absentee")]
        public string Absentee { get; set; }

        [SheetField("PaidPmi")]
        public string PaidPmi { get; set; }

        [SheetField("CheckNumber")]
        public string CheckNumber { get; set; }

        [SheetField("TransactionNumber")]
        public string TransactionNumber { get; set; }

        [SheetField("PayPalFee")]
        public decimal? PayPalFee { get; set; }

        [SheetField("TotalPayPal")]
        public decimal? TotalPayPal { get; set; }

        [SheetField("NetPayPal")]
        public decimal? NetPayPal { get; set; }

        [SheetField("TotalCheck")]
        public decimal? TotalCheck { get; set; }

        [SheetField("TotalCash")]
        public decimal? TotalCash { get; set; }

        [SheetField("TotalCreditCard")]
        public decimal? TotalCreditCard { get; set; }

        [SheetField("TotalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}
