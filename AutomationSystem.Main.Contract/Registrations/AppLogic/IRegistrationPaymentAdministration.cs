using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Payments;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic
{
    public interface IRegistrationPaymentAdministration
    {
        RegistrationPaymentDetailPageModel GetRegistrationPaymentByRegistrationId(long registrationId);
        
        RegistrationPaymentForEdit GetRegistrationPaymentForEditById(long registrationId);
        
        RegistrationPaymentForEdit GetRegistrationPaymentForEditByForm(RegistrationPaymentForm form);
        
        void SaveRegistrationPayment(RegistrationPaymentForm form);
    }
}
