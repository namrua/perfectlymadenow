using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.Persons.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.DistanceProfiles.AppLogic.EventCheckers
{
    public class PersonToDeleteHasNoDistanceProfileEventChecker : IEventChecker<PersonDeletingEvent>
    {
        public IDistanceProfileDatabaseLayer distanceProfileDb;

        public PersonToDeleteHasNoDistanceProfileEventChecker(IDistanceProfileDatabaseLayer distanceProfileDb)
        {
            this.distanceProfileDb = distanceProfileDb;
        }

        public bool CheckEvent(PersonDeletingEvent evnt)
        {
            var result = !distanceProfileDb.PersonOnAnyDistanceProfile(evnt.PersonId);
            return result;
        }
    }
}
