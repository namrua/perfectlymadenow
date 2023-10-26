using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Main.Contract.Profiles.AppLogic;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Profiles.Data;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Core.Profiles.System.Extensions;
using AutomationSystem.Shared.Contract;
using AutomationSystem.Shared.Contract.Identities.AppLogic.Models;
using AutomationSystem.Shared.Contract.Identities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Profile = AutomationSystem.Main.Model.Profile;

namespace AutomationSystem.Main.Core.Profiles.AppLogic
{
    public class ProfileUsersAdministration : IProfileUsersAdministration
    {
        private readonly IProfileDatabaseLayer profileDb;
        private readonly IIdentityDatabaseLayer identityDb;
        private readonly IIdentityResolver identityResolver;
        private readonly ICoreMapper coreMapper;


        // constructor
        public ProfileUsersAdministration(
            IProfileDatabaseLayer profileDb,
            IIdentityDatabaseLayer identityDb,
            IIdentityResolver identityResolver,
            ICoreMapper coreMapper)
        {
            this.profileDb = profileDb;
            this.identityDb = identityDb;
            this.identityResolver = identityResolver;
            this.coreMapper = coreMapper;
        }

        // gets profile users page model
        public ProfileUsersPageModel GetProfileUsersPageModel(long profileId)
        {
            var profile = GetProfileById(profileId, ProfileIncludes.ProfileUsers);
            identityResolver.CheckEntitleForProfile(Entitle.MainProfiles, profile);
            var users = identityDb.GetUsers().Select(coreMapper.Map<UserShortDetail>).ToList();
            var userIdsInProfile = new HashSet<int>(profile.ProfileUsers.Select(x => x.UserId));

            var result = new ProfileUsersPageModel
            {
                ProfileId = profileId,
                AssignedUsers = users.Where(x => userIdsInProfile.Contains(x.UserId)).ToList(),
                UnassignedUsers = users.Where(x => !userIdsInProfile.Contains(x.UserId)).ToList(),
            };
            return result;
        }

        // adds user to profile
        public void AddUserToProfile(long profileId, int userId)
        {
            identityResolver.CheckEntitleForProfileId(Entitle.MainProfiles, profileId);
            profileDb.AddProfileUser(profileId, userId);
        }

        // removes user from profile
        public void RemoveUserFromProfile(long profileId, int userId)
        {
            identityResolver.CheckEntitleForProfileId(Entitle.MainProfiles, profileId);
            profileDb.RemoveProfileUser(profileId, userId);
        }

        #region private methods
        // gets profile, checks whether profile exists
        private Profile GetProfileById(long profileId, ProfileIncludes includes = ProfileIncludes.None)
        {
            var result = profileDb.GetProfileById(profileId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Profile with id {profileId}.");
            }

            return result;
        }
        #endregion
    }
}
