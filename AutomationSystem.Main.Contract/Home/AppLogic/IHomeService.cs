using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Home.AppLogic.Models;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;

namespace AutomationSystem.Main.Contract.Home.AppLogic
{
    /// <summary>
    /// Provides services for home controller 
    /// </summary>
    public interface IHomeService
    {

        // gets home page model
        HomePageModel GetHomePageModel(EnvironmentTypeEnum? env, string profileMoniker);

        // gets distance classes page model
        DistanceClassesPageModel GetDistanceClassesPageModel(EnvironmentTypeEnum? env, string profileMoniker);

        // gets class registration selection page model
        ClassRegistrationSelectionPageModel GetClassRegistrationSelectionPageModel(long classId, bool? forWwa, RegistrationTypeEnum? backFromRegistrationTypeId);

        // gets registration confirmation page model
        RegistrationConfirmationPageModel GetRegistrationConfirmationPageModel(long registrationId);

        #region registration page style getting

        // gets registration page style by profileId - CAUSES NO EXCEPTIONS!
        RegistrationPageStyle GetRegistrationPageStyleByProfileId(long profileId);

        // gets registration page style by classId - CAUSES NO EXCEPTIONS!
        RegistrationPageStyle GetRegistrationPageStyleByClassId(long? classId);

        // gets registration page style by registrationId - CAUSES NO EXCEPTIONS!
        RegistrationPageStyle GetRegistrationPageStyleByRegistrationId(long? classRegistrationId);

        #endregion

        #region registration editing

        // gets controller info
        RegistrationControllerInfo GetControllerInfoByRegistrationTypeId(RegistrationTypeEnum registrationTypeId);   

        // gets new registration for edit
        IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEdit(RegistrationTypeEnum registrationTypeId, long classId);

        // gets new registration for edit by invitation request
        IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEditByInvitationRequest(string invitationRequest);

        // gets registration for edit by registration id
        IRegistrationForEdit<BaseRegistrationForm> GetRegistrationForEdit(long registrationId);

        // gets registration for edit based on form
        IRegistrationForEdit<BaseRegistrationForm> GetFormRegistrationForEdit(BaseRegistrationForm form);

        // saves registration
        HomeWorkflowState SaveRegistration(BaseRegistrationForm form);

        #endregion

        #region reviewing

        // gets former student selection page model
        FormerStudentSelectionPageModel GetFormerStudentSelectionPageModel(long registrationId);

        // saves registration last class
        long SaveRegistrationLastClass(RegistrationLastClassForm form);

        // saves former student selection
        void SaveFormerStudentSelection(long registrationId, long? formerStudentId);

        #endregion

        #region agreement

        // gets agreement registration page model
        RegistrationAgreementPageModel GetRegistrationAgreementPageModel(long registrationId);

        // saves registration acceptiation state, returns home workflow state
        HomeWorkflowState SaveRegistrationAgreementAcceptationState(RegistrationAgreementForm form);

        #endregion

        #region payment

        // get registration payment page model
        RegistrationPaymentPageModel GetRegistrationPaymentPageModel(long registrationId);

        // executes payment
        PaymentResult ExecutePayment(PaymentExecutionInput input);

        #endregion
    }
}
