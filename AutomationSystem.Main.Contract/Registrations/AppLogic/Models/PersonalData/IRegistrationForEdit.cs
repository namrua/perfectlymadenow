using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Registration for edit interface (enables make TForm covariant)
    /// </summary>
    /// <typeparam name="TForm">Registration form type</typeparam>
    public interface IRegistrationForEdit<out TForm> where TForm : BaseRegistrationForm
    {
      
        List<IEnumItem> Countries { get; }
        List<IEnumItem> Languages { get; }
        TForm Form { get; }

    }

}
