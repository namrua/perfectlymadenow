using AutomationSystem.Base.Contract.Identities.Models.Events;
using AutomationSystem.Shared.Core.ConferenceAccounts.Data;
using PerfectlyMadeInc.DesignTools.Contract.Events;

namespace AutomationSystem.Shared.Core.ConferenceAccounts.AppLogic.EventCheckers
{
    public class UserGroupToDeleteHasNoConferenceAccountEventChecker : IEventChecker<UserGroupDeletingEvent>
    {
        public IConferenceAccountDatabaseLayer conferenceDb;

        public UserGroupToDeleteHasNoConferenceAccountEventChecker(IConferenceAccountDatabaseLayer conferenceDb)
        {
            this.conferenceDb = conferenceDb;
        }

        public bool CheckEvent(UserGroupDeletingEvent evnt)
        {
            var result = !conferenceDb.AnyConferenceAccountOnUserGroup(evnt.UserGroupId, evnt.UserGroupTypeId);
            return result;
        }
    }
}
