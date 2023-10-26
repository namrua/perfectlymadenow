using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic
{
    public interface IRegistrationAdministration
    {

        #region registration views

        ClassRegistrationPageModel GetClassRegistrationPageModel(long classId);
        
        RegistrationDetailPageModel GetRegistrationDetailPageModel(long registrationId);
        
        RegistrationsForList GetRegistrationsForList(RegistrationFilter filter, bool search);
        
        RegistrationCommunicationPageModel GetRegistrationCommunication(long registrationId);

        #endregion
        
        #region commands
        
        void ApproveRegistration(long registrationId);
        
        long? DeleteRegistration(long registrationId);
        
        void CreateRegistrationCancelationEmailTemplate(long registrationId);
        
        void CancelRegistration(long registrationId);
        
        void CancelRegistrationWithoutNotification(long registrationId);
        
        void DiscardCancelation(long registrationId);
        
        void ManualReview(long registrationId, long formerStudentId);
        
        void SendPaymentRequest(long id);

        #endregion

    }
}