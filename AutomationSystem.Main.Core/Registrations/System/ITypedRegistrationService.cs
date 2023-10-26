using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System
{
    /// <summary>
    /// Registration typed service provides getting typed detail and forms
    /// </summary>   
    public interface ITypedRegistrationService<TForm, TDetail> : IRegistrationService 
        where TForm : BaseRegistrationForm
        where TDetail : BaseRegistrationDetail
    {
        
        TDetail GetRegistrationDetailTyped(ClassRegistration registration);
        
        RegistrationForEdit<TForm> GetNewRegistrationForEditTyped(RegistrationTypeEnum registrationTypeId, Class cls);
        
        RegistrationForEdit<TForm> GetRegistrationForEditTyped(ClassRegistration registration);
        
        RegistrationForEdit<TForm> GetFormRegistrationForEditTyped(TForm form);
        
        long SaveRegistrationTyped(TForm form, bool insertAsTeporary, ApprovementTypeEnum approvementType);

    }
}
