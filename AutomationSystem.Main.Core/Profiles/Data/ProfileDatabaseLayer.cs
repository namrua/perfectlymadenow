using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutomationSystem.Main.Contract.Profiles.AppLogic.Models;
using AutomationSystem.Main.Core.Profiles.Data.Extensions;
using AutomationSystem.Main.Core.Profiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;

namespace AutomationSystem.Main.Core.Profiles.Data
{

    /// <summary>
    /// Provides data layer for Profile related entities
    /// </summary>
    public class ProfileDatabaseLayer : IProfileDatabaseLayer
    {

        #region profile

        // gets profiles by filter
        public List<Profile> GetProfilesByFilter(ProfileFilter filter = null, ProfileIncludes includes = ProfileIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.Profiles.AddIncludes(includes).Filter(filter).ToList();
                result = result.Select(x => ProfileRemoveInactive.RemoveInactiveForProfile(x, includes)).ToList();
                return result;
            }
        }

        // gets profile by id
        public Profile GetProfileById(long profileId, ProfileIncludes includes = ProfileIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.Profiles.AddIncludes(includes).Active().FirstOrDefault(x => x.ProfileId == profileId);
                result = ProfileRemoveInactive.RemoveInactiveForProfile(result, includes);
                return result;
            }
        }

        // gets profile by moniker
        public Profile GetProfileByMoniker(string profileMoniker, ProfileIncludes includes = ProfileIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.Profiles.AddIncludes(includes).Active().FirstOrDefault(x => x.Moniker.ToLower() == profileMoniker.ToLower());
                result = ProfileRemoveInactive.RemoveInactiveForProfile(result, includes);
                return result;
            }
        }


        // inserts profile
        public long InsertProfile(Profile profile)
        {
            using (var context = new MainEntities())
            {
                context.Profiles.Add(profile);
                context.SaveChanges();
                return profile.ProfileId;
            }
        }

        
        // update profile
        public void UpdateProfile(Profile profile)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.Profiles.Active().FirstOrDefault(x => x.ProfileId == profile.ProfileId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no Profile with id {profile.ProfileId}.");

                toUpdate.Name = profile.Name;
                toUpdate.Moniker = profile.Moniker;
                context.SaveChanges();
            }
        }


        // deletes profile
        public void DeleteProfile(long profileId)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.Profiles.Active().FirstOrDefault(x => x.ProfileId == profileId);
                if (toDelete == null) return;
                context.Profiles.Remove(toDelete);
                context.SaveChanges();
            }
        }

        #endregion


        #region profile users

        // adds Profile-User relation
        public long AddProfileUser(long profileId, int userId)
        {
            using (var context = new MainEntities())
            {
                // checks whether relation does not exist
                var profileUser = context.ProfileUsers.Active().FirstOrDefault(x => x.ProfileId == profileId && x.UserId == userId);
                if (profileUser != null)
                    return profileUser.ProfileUserId;

                // adds relation
                var toInsert = new ProfileUser
                {
                    ProfileId = profileId,
                    UserId = userId,
                };
                context.ProfileUsers.Add(toInsert);
                context.SaveChanges();
                return toInsert.ProfileId;
            }
        }


        // removes Profile-User relation
        public void RemoveProfileUser(long profileId, int userId)
        {
            using (var context = new MainEntities())
            {
                // loads relation and deletes it
                var toDelete = context.ProfileUsers.Active().FirstOrDefault(x => x.ProfileId == profileId && x.UserId == userId);
                if (toDelete == null) return;
                context.ProfileUsers.Remove(toDelete);
                context.SaveChanges();
            }
        }

        #endregion


        #region identities

        // gets profiles assigned to user
        public async Task<long[]> GetProfilesAssignedToUserIdAsync(int userId)
        {
            using (var context = new MainEntities())
            {
                var result = await context.ProfileUsers.Active()
                    .Where(x => !x.Profile.Deleted && x.UserId == userId)
                    .Select(x => x.ProfileId).Distinct().ToArrayAsync();
                return result;
            }
        }

        #endregion

    }

}
