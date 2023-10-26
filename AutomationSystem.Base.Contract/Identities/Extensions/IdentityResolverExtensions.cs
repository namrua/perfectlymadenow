using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Base.Contract.Integration;

namespace AutomationSystem.Base.Contract.Identities.Extensions
{

    /// <summary>
    /// Identity resolver extensions
    /// </summary>
    public static class IdentityResolverExtensions
    {

        // checks entitle for ConferenceAccountInfo
        public static void CheckEntitleForConferenceAccountInfo(this IIdentityResolver identityResolver,
            Entitle entitle, ConferenceAccountInfo conferenceAccount)
        {
            identityResolver.CheckEntitleForUserGroup(entitle, conferenceAccount.UserGroupTypeId, conferenceAccount.UserGroupId,
                entityType: "ConferenceAccount", entityId: conferenceAccount.ConferenceAccountId);
        }

    }

}
