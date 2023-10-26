using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.Profiles.AppLogic.Models.Events
{
    public class ProfileDeletingEvent : BaseEvent
    {
        public long ProfileId { get; }

        public ProfileDeletingEvent(long profileId)
        {
            ProfileId = profileId;
        }

        public override string ToString()
        {
            return $"ProfileId: {ProfileId}";
        }
    }
}
