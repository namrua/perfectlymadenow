using AutomationSystem.Main.Contract.Persons.AppLogic;

namespace AutomationSystem.Main.Contract.Preferences.AppLogic.Models
{


    /// <summary>
    /// Izi preferences for edit
    /// </summary>
    public class IziPreferenceForEdit
    {

        public IziPreferenceForm Form { get; set; } = new IziPreferenceForm();
        public IPersonHelper PersonHelper { get; set; } = new EmptyPersonHelper();

    }

}
