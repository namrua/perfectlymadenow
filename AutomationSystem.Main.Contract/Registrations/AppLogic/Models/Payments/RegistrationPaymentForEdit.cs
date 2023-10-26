namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments
{
    /// <summary>
    /// Registration payment for edit
    /// </summary>
    public class RegistrationPaymentForEdit
    {
        public string CurrencyCode { get; set; }
        public RegistrationPaymentForm Form { get; set; } = new RegistrationPaymentForm();

    }
}
