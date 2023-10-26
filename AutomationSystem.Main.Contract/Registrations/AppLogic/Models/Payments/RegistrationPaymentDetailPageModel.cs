using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments
{
    /// <summary>
    /// Registration payment detail page model
    /// </summary>
    public class RegistrationPaymentDetailPageModel
    {       

        public long ClassId { get; set; }        
        public ClassState ClassState { get; set; }        
        public long ClassRegistrationId { get; set; }
        public RegistrationPaymentDetail Detail { get; set; }
        public PayPalRecordDetail PayPalRecord { get; set; }                // nullable
        public string CurrencyCode { get; set; }
        public bool CanEdit { get; set; }

        // constructor
        public RegistrationPaymentDetailPageModel()
        {
            Detail = new RegistrationPaymentDetail();
        }

    }

}
