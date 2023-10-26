using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.Classes.AppLogic.EventCheckers
{
    public class ProfileToDeleteHasNoClassEventChecker : IEventChecker<ProfileDeletingEvent>
    {
        private readonly IClassDatabaseLayer classDb;

        public ProfileToDeleteHasNoClassEventChecker(IClassDatabaseLayer classDb)
        {
            this.classDb = classDb;
        }

        public bool CheckEvent(ProfileDeletingEvent evnt)
        {
            var result = !classDb.AnyClassOnProfile(evnt.ProfileId);
            return result;
        }
    }
}
