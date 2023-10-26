using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System
{
    /// <summary>
    /// Registration typed service provides getting typed detail and forms
    /// </summary>
    public interface IRegistrationService
    {        
        BaseRegistrationDetail GetRegistrationDetail(ClassRegistration registration);
        
        IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEdit(RegistrationTypeEnum registrationTypeId, Class cls);
        
        IRegistrationForEdit<BaseRegistrationForm> GetRegistrationForEdit(ClassRegistration registration);
        
        IRegistrationForEdit<BaseRegistrationForm> GetFormRegistrationForEdit(BaseRegistrationForm form);
        
        long SaveRegistration(BaseRegistrationForm form, bool insertAsTeporary, ApprovementTypeEnum approvementType);

    }
}
