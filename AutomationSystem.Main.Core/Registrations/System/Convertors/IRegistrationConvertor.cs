using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;

namespace AutomationSystem.Main.Core.Registrations.System.Convertors
{
    /// <summary>
    /// Converts registration objects - base
    /// </summary>
    public interface IRegistrationConvertor
    {
        
        RegistrationFormTypeEnum RegistrationFormType { get; }

        RegistrationControllerInfo ControllerInfo { get; }

    }
}
