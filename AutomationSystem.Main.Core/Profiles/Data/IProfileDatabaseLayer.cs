using System.Collections.Generic;
using System.Threading.Tasks;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Profiles.Data
{
    /// <summary>
    /// Provides data layer for Profile related entities
    /// </summary>
    public interface IProfileDatabaseLayer
    {

        #region profile

        // gets profiles by filter
        List<Profile> GetProfilesByFilter(ProfileFilter filter = null, ProfileIncludes includes = ProfileIncludes.None);

        // gets profile by id
        Profile GetProfileById(long profileId, ProfileIncludes includes = ProfileIncludes.None);

        // gets profile by moniker
        Profile GetProfileByMoniker(string profileMoniker, ProfileIncludes includes = ProfileIncludes.None);

        // inserts profile
        long InsertProfile(Profile profile);

        // update profile
        void UpdateProfile(Profile profile);

        // deletes profile
        void DeleteProfile(long profileId);

        #endregion


        #region profile users

        // adds Profile-User relation
        long AddProfileUser(long profileId, int userId);

        // removes Profile-User relation
        void RemoveProfileUser(long profileId, int userId);

        #endregion


        #region identities

        // gets profiles assigned to user
        Task<long[]> GetProfilesAssignedToUserIdAsync(int userId);

        #endregion

    }

}
