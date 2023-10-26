using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.System.Convertors
{
    /// <summary>
    /// Provides base conversion of class registration entities
    /// </summary>
    public interface IBaseRegistrationConvertor
    {
        void FillRegistrationForEdit<TForm>(RegistrationForEdit<TForm> registrationForEdit, Class cls)
            where TForm : BaseRegistrationForm;       
        
        void FillRegistrationForm(BaseRegistrationForm form, ClassRegistration registration);
        
        void FillRegistrationDetail(BaseRegistrationDetail detail, ClassRegistration registration);
        
        ClassRegistration ConvertToClassRegistration(BaseRegistrationForm form);
        
        RegistrationListItem ConvertToRegistrationListItem(ClassRegistration registration);
    }
}
