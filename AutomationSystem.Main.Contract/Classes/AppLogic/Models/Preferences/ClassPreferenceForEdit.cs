using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences
{
    /// <summary>
    /// Class preference for edit
    /// </summary>
    public class ClassPreferenceForEdit
    {

        public List<RegistrationColorScheme> ColorSchemes { get; set; } = new List<RegistrationColorScheme>();
        public List<IEnumItem> Currencies { get; set; } = new List<IEnumItem>();
        public ClassPreferenceForm Form { get; set; } = new ClassPreferenceForm();
        public IPersonHelper PersonHelper { get; set; } = new EmptyPersonHelper();

    }
}
