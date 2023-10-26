using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;

namespace AutomationSystem.Main.Core.Registrations.System
{
    public interface IRegistrationPersonalDataService
    {
        long SaveRegistration(BaseRegistrationForm form);
    }
}
