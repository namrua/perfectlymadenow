using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Documents;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic
{
    public interface IRegistrationDocumentAdministration
    {
        void GenerateCertificate(string rootPath, long registrationId);
        
        RegistrationDocumentsPageModel GetRegistrationDocumentsPageModel(long registrationId);
    }
}
