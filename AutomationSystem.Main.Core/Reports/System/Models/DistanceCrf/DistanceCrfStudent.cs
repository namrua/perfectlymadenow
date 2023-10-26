using SheetUtility.Interfaces;

namespace AutomationSystem.Main.Core.Reports.System.Models.DistanceCrf
{
    /// <summary>
    /// Distance CRF student
    /// </summary>
    public class DistanceCrfStudent
    {
        // Class

        [SheetField("ClassDate")]
        public string ClassDate { get; set; }

        [SheetField("ClassLocation")]
        public string Location { get; set; }


        // Registration

        [SheetField("RegistrantLastName")]
        public string RegistrantLastName { get; set; }

        [SheetField("RegistrantFirstName")]
        public string RegistrantFirstName { get; set; }

        [SheetField("RegistrantAddress")]
        public string RegistrantAddress { get; set; }

        [SheetField("RegistrantEmail")]
        public string RegistrantEmail { get; set; }


        [SheetField("ParticipantLastName")]
        public string ParticipantLastName { get; set; }

        [SheetField("ParticipantFirstName")]
        public string ParticipantFirstName { get; set; }

        [SheetField("ParticipantAddress")]
        public string ParticipantAddress { get; set; }

        [SheetField("ParticipantCountry")]
        public string ParticipantCountry { get; set; }


        [SheetField("RegistrationDate")]
        public string RegistrationDate { get; set; }

        // Payment

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
