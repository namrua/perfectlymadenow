using System.Collections.Generic;
using System.Linq;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.Classes.AppLogic.Models.Events
{
    /// <summary>
    /// Class person echanged event
    /// </summary>
    public class ClassPersonsChangedEvent : BaseEvent
    {
        public long ClassId { get; }
        public List<PersonIdAndRole> CurrentPersons { get; set; } = new List<PersonIdAndRole>();
        public List<PersonIdAndRole> AddedPersons { get; set; } = new List<PersonIdAndRole>();
        public List<PersonIdAndRole> RemovedPersons { get; set; } = new List<PersonIdAndRole>();

        public ClassPersonsChangedEvent(long classId)
        {
            ClassId = classId;
        }

        public override string ToString()
        {
            return $"CurrentPersons [{string.Join(", ", CurrentPersons.Select(x => $"({x})"))}]; "
                   + $"AddedPersons [{string.Join(", ", AddedPersons.Select(x => $"({x})"))}]; "
                   + $"RemovedPersons [{string.Join(", ", RemovedPersons.Select(x => $"({x})"))}]; ";
        }
    }
}
