using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.Convertors
{
    /// <summary>
    /// Converts registration last class objects
    /// </summary>
    public interface IRegistrationLastClassConvertor
    {

        RegistrationLastClassForm ConvertToRegistrationLastClassForm(long registrationId, ClassRegistrationLastClass lastClass);
        
        RegistrationLastClassDetail ConvertToRegistrationLastClassDetail(ClassRegistrationLastClass lastClass);
        
        ClassRegistrationLastClass ConvertToClassRegistrationLastClass(RegistrationLastClassForm form);

    }

}
