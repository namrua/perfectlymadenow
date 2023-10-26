using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System.Convertors
{
    public interface ITypedRegistrationConvertor<TForm, TDetail> : IRegistrationConvertor
        where TForm : BaseRegistrationForm
        where TDetail : BaseRegistrationDetail
    {
        
        RegistrationForEdit<TForm> InitializeRegistrationForEdit(Class cls);
        
        TForm InitializeRegistrationForm(RegistrationTypeEnum registrationTypeId, long classId);
        
        TForm ConvertToRegistrationForm(ClassRegistration registration);
        
        TDetail ConvertToRegistrationDetail(ClassRegistration registration);
        
        ClassRegistration ConvertToClassRegistration(TForm form);

    }
}
