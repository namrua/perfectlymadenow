namespace AutomationSystem.Main.Contract.Home.AppLogic.Models
{
    /// <summary>
    /// Determines home workflow state
    /// </summary>
    public enum HomeWorkflowStage
    {
        ClassSelection,
        RegistrationTypeSelection,
        PersonalDataForm,                           // GetRegistrationForEdit
        PersonalDataSave,                           // SaveRegistration
        FormerStudentSelection,                     // GetFormerStudentSelectionPageModel | SaveFormerStudentSelection
        Confirmation,                               // GetRegistrationConfirmationPageModel
        Agreement,                                  // GetRegistrationAgreementPageModel
        AgreementAcceptation,                       // SaveRegistrationAgreementAcceptationState
        Payment,                                    // ExecutePayment | GetRegistrationPaymentPageModel
        ManualReviewing,
        Complete
    }
}