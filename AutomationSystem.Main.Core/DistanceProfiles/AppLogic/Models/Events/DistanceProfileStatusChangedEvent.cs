using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Main.Core.DistanceProfiles.AppLogic.Models.Events
{
    public class DistanceProfileStatusChangedEvent : BaseEvent
    {
        public long DistanceProfileId { get; }
        public bool IsActive { get; }

        public DistanceProfileStatusChangedEvent(long distanceProfileId, bool isActive)
        {
            DistanceProfileId = distanceProfileId;
            IsActive = isActive;
        }

        public override string ToString()
        {
            return $"DistanceProfileId: {DistanceProfileId}, IsActive: {IsActive}";
        }
    }
}
