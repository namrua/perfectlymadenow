using System.Threading.Tasks;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Shared.Contract.Identities.System;
using AutomationSystem.Shared.Contract.Identities.System.Models;

namespace AutomationSystem.Main.Core.Profiles.System
{
    /// <summary>
    /// Provides User profile membership for identity infrastructure
    /// </summary>
    public class ProfileMembershipProvider : IUserGroupMembershipProvider
    {

        private readonly IProfileDatabaseLayer profileDb;

        // constructor
        public ProfileMembershipProvider(IProfileDatabaseLayer profileDb)
        {
            this.profileDb = profileDb;
        }


        // gets UserGroup memberships for user id
        public async Task<UserGroupMembership> GetUserGroupMembershipAsync(int userId)
        {
            var profileIds = await profileDb.GetProfilesAssignedToUserIdAsync(userId);
            var result = new UserGroupMembership(UserGroupTypeEnum.MainProfile, profileIds);
            return result;
        }

    }

}
