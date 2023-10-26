using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Main.Core.DistanceProfiles.AppLogic.EventCheckers
{
    public class ProfileToDeleteHasNoDistanceProfileEventChecker : IEventChecker<ProfileDeletingEvent>
    {
        public IDistanceProfileDatabaseLayer distanceProfileDb;

        public ProfileToDeleteHasNoDistanceProfileEventChecker(IDistanceProfileDatabaseLayer distanceProfileDb)
        {
            this.distanceProfileDb = distanceProfileDb;
        }

        public bool CheckEvent(ProfileDeletingEvent evnt)
        {
            var result = !distanceProfileDb.AnyDistanceProfileOnProfile(evnt.ProfileId);
            return result;
        }
    }
}
