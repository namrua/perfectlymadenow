namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Registration payment page model
    /// </summary>
    public class RegistrationPaymentPageModel
    {
        public long ClassRegistrationId { get; set; }
        public ClassPublicDetail Class { get; set; }
        public RegistrationTypeListItem RegistrationType { get; set; }
        public PayPalTransactionInfo PayPalTransactionInfo { get; set; }
        public string PaymentErrorMessage { get; set; }
        public HomeWorkflowState WorkflowState { get; set; }

        // constructor
        public RegistrationPaymentPageModel()
        {
            Class = new ClassPublicDetail();
            RegistrationType = new RegistrationTypeListItem();      
            WorkflowState = new HomeWorkflowState();            
        }
    }
}