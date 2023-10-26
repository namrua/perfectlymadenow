using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Models.Events
{
    /// <summary>
    /// Event is raised when new class is created
    /// </summary>
    public class ClassCreatedEvent : BaseEvent
    {
        public long ClassId { get; }

        public ClassCreatedEvent(long classId)
        {
            ClassId = classId;
        }

        public override string ToString()
        {
            return $"ClassId: {ClassId}";
        }
    }
}
