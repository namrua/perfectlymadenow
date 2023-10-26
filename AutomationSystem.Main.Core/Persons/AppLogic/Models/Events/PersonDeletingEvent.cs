using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.Persons.AppLogic.Models.Events
{
    public class PersonDeletingEvent : BaseEvent
    {
        public long PersonId { get; }

        public PersonDeletingEvent(long personId)
        {
            PersonId = personId;
        }

        public override string ToString()
        {
            return $"PersonId: {PersonId}";
        }
    }
}
