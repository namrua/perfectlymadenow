using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic
{
    public interface IRegistrationPersonalDataAdministration
    {
        RegistrationControllerInfo GetControllerInfoByRegistrationTypeId(RegistrationTypeEnum registrationTypeId);
        
        BaseRegistrationDetail GetRegistrationDetailById(long registrationId);
        
        IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEdit(RegistrationTypeEnum registrationTypeId, long classId);
        
        IRegistrationForEdit<BaseRegistrationForm> GetRegistrationForEdit(long registrationId);
        
        IRegistrationForEdit<BaseRegistrationForm> GetFormRegistrationForEdit(BaseRegistrationForm form);
        
        long SaveRegistration(BaseRegistrationForm form);
    }
}
