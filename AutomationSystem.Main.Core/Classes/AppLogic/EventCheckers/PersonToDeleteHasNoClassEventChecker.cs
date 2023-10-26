using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.Classes.AppLogic.EventCheckers
{
    public class PersonToDeleteHasNoClassEventChecker : IEventChecker<PersonDeletingEvent>
    {
        public IClassDatabaseLayer classDb;

        public PersonToDeleteHasNoClassEventChecker(IClassDatabaseLayer classDb)
        {
            this.classDb = classDb;
        }

        public bool CheckEvent(PersonDeletingEvent evnt)
        {
            var result = !classDb.PersonOnAnyClass(evnt.PersonId);
            return result;
        }
    }
}
