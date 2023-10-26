using AutomationSystem.Base.Contract.Enums;
using PerfectlyMadeInc.DesignTools.Contract.Events.Models;

namespace AutomationSystem.Base.Contract.Identities.Models.Events
{
    public class UserGroupDeletingEvent : BaseEvent
    {
        public long UserGroupId { get; }
        public UserGroupTypeEnum UserGroupTypeId { get; }

        public UserGroupDeletingEvent(long userGroupId, UserGroupTypeEnum userGroupTypeId)
        {
            UserGroupId = userGroupId;
            UserGroupTypeId = userGroupTypeId;
        }

        public override string ToString()
        {
            return $"UserGroupId: {UserGroupId}, UserGroupTypeId: {UserGroupTypeId}";
        }
    }
}
