using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Registrations.AppLogic.Convertors
{

    /// <summary>
    /// Converts registration last class objects
    /// </summary> 
    public class RegistrationLastClassConvertor : IRegistrationLastClassConvertor
    {
        public RegistrationLastClassForm ConvertToRegistrationLastClassForm(long registrationId, ClassRegistrationLastClass lastClass)
        {
            var result = new RegistrationLastClassForm
            {
                ClassRegistrationId = registrationId,
                Location = lastClass.Location,
                Month = lastClass.Month,
                Year = lastClass.Year
            };
            return result;
        }
        
        public RegistrationLastClassDetail ConvertToRegistrationLastClassDetail(ClassRegistrationLastClass lastClass)
        {
            var result = new RegistrationLastClassDetail
            {
                Location = lastClass.Location,
                Month = lastClass.Month,
                Year = lastClass.Year
            };
            return result;
        }
        
        public ClassRegistrationLastClass ConvertToClassRegistrationLastClass(RegistrationLastClassForm form)
        {
            var result = new ClassRegistrationLastClass
            {
                Location = form.Location,
                Month = form.Month ?? 0,
                Year = form.Year ?? 0
            };
            return result;
        }
    }
}
