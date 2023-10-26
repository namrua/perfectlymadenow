using System.Threading.Tasks;
using AutomationSystem.Shared.Contract.Identities.System.Models;

namespace AutomationSystem.Shared.Contract.Identities.System
{
    /// <summary>
    /// Provides UserGroup membership
    /// </summary>
    public interface IUserGroupMembershipProvider
    {

        // gets UserGroup memberships for user id
        Task<UserGroupMembership> GetUserGroupMembershipAsync(int userId);

    }

}
