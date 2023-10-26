using System.Collections.Generic;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.StyleAndBehavior
{
    /// <summary>
    /// Class style for edit
    /// </summary>
    public class ClassStyleForEdit 
    {
        public bool ShowClassBehaviorSettings { get; set; }
        public List<RegistrationColorScheme> ColorSchemes { get; set; } = new List<RegistrationColorScheme>();
        public ClassStyleForm Form { get; set; } = new ClassStyleForm();
    }
}