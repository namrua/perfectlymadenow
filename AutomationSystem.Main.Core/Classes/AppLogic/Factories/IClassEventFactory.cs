using AutomationSystem.Main.Core.Classes.AppLogic.Models.Events;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Factories
{
    /// <summary>
    /// Factory for class events
    /// </summary>
    public interface IClassEventFactory
    {
        ClassPersonsChangedEvent CreateClassPersonsChangedEventWhenChanged(Class originClass, Class updatedClass);
    }
}
