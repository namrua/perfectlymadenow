using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Contract.Identities.Data;
using AutomationSystem.Shared.Model;
using AutomationSystem.Shared.Model.Queries;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Shared.Core.Identities.Data
{
    /// <summary>
    /// Provides IDM database layer
    /// </summary>
    public class IdentityDatabaseLayer : IIdentityDatabaseLayer
    {

        // gets list of users
        public List<User> GetUsers()
        {
            using (var context = new CoreEntities())
            {
                var result = context.Users.Active().ToList();
                return result;
            }
        }


        // gets users role types
        public List<UserRoleType> GetUserRoleTypes()
        {
            using (var context = new CoreEntities())
            {
                var result = context.UserRoleTypes.ToList();
                return result;
            }
        }


        // get user by id
        public User GetUserById(int userId)
        {
            using (var context = new CoreEntities())
            {
                var result = context.Users.Include("UserRoleAssignments").Active().FirstOrDefault(x => x.UserId == userId);
                if (result != null)                
                    result.UserRoleAssignments = result.UserRoleAssignments.AsQueryable().Active().ToList();                
                return result;
            }
        }


        // checks user data for duplicities
        public void CheckUserDuplicities(int userId, string name, string googleAccount, 
            out bool hasDuplicitName, out bool hasDuplicitGoogleAccount)
        {
            using (var context = new CoreEntities())
            {
                hasDuplicitName = context.Users.Active()
                    .Any(x => x.UserId != userId && x.Name.ToLower() == name.ToLower());
                hasDuplicitGoogleAccount = context.Users.Active()
                    .Any(x => x.UserId != userId && x.GoogleAccount.ToLower() == googleAccount.ToLower());
            }
        }


        // inserts user
        public int InsertUser(User user)
        {
            using (var context = new CoreEntities())
            {
                context.Users.Add(user);
                context.SaveChanges();
                return user.UserId;
            }
        }

        // update user
        public void UpdateUser(User user, HashSet<UserRoleTypeEnum> roleRestrictions, bool deleteLoginsWhenGoogleAccountIsChanged = false)
        {
            using (var context = new CoreEntities())
            {
                var toUpdate = context.Users.Include("UserRoleAssignments").Active().FirstOrDefault(x => x.UserId == user.UserId);
                if (toUpdate == null)
                    throw new ArgumentException($"There is no User with id {user.UserId}.");

                // updates user
                toUpdate.Active = user.Active;
                toUpdate.Name = user.Name;                

                // updates roles
                var userRoles = toUpdate.UserRoleAssignments.AsQueryable().Active();
                if (roleRestrictions != null)
                    userRoles = userRoles.Where(x => roleRestrictions.Contains(x.UserRoleTypeId));
                var updateResolver = new SetUpdateResolver<UserRoleAssignment, UserRoleTypeEnum>(x => x.UserRoleTypeId, (o, n) => { });
                var strategy = updateResolver.ResolveStrategy(userRoles, user.UserRoleAssignments);
                context.UserRoleAssignments.AddRange(strategy.ToAdd);
                context.UserRoleAssignments.RemoveRange(strategy.ToDelete);

                // updates google account and deletes logins when google account is changed
                bool isGoogleAccountChanged = toUpdate.GoogleAccount.ToLower() != user.GoogleAccount.ToLower();
                toUpdate.GoogleAccount = user.GoogleAccount;
                if (isGoogleAccountChanged && deleteLoginsWhenGoogleAccountIsChanged)
                {
                    var toDelete = context.UserLogins.Active().Where(x => x.UserId == user.UserId).ToList();
                    context.UserLogins.RemoveRange(toDelete);
                }

                // saves data
                context.SaveChanges();
            }
        }

        // delete user
        public void DeleteUser(int userId)
        {
            using (var context = new CoreEntities())
            {
                var toDelete = context.Users.FirstOrDefault(x => x.UserId == userId);
                if (toDelete == null) return;

                context.Users.Remove(toDelete);
                context.SaveChanges();
            }
        }
    }

}
