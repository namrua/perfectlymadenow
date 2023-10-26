using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.Persons.AppLogic.EventCheckers
{
    public class ProfileToDeleteHasNoPersonEventChecker : IEventChecker<ProfileDeletingEvent>
    {
        private readonly IPersonDatabaseLayer personDb;

        public ProfileToDeleteHasNoPersonEventChecker(IPersonDatabaseLayer personDb)
        {
            this.personDb = personDb;
        }

        public bool CheckEvent(ProfileDeletingEvent evnt)
        {
            var result = !personDb.AnyPersonOnProfile(evnt.ProfileId);
            return result;
        }
    }
}
