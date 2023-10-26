using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Factories
{
    /// <summary>
    /// Creates ClassPreference related objects
    /// </summary>
    public interface IClassPreferenceFactory
    {
        ClassPreference CreateDefaultClassPreference();
        ClassPreferenceForEdit CreateClassPreferenceForEdit(long profileId);
    }
}
