using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Identities.Data
{
    /// <summary>
    /// Provides IDM database layer
    /// </summary>
    public interface IIdentityDatabaseLayer
    {

        // gets list of users
        List<User> GetUsers();

        // gets users role types
        List<UserRoleType> GetUserRoleTypes();       

        // get user by id
        User GetUserById(int userId);

        // checks user data for duplicities
        void CheckUserDuplicities(int userId, string name, string googleAccount, out bool hasDuplicitName, out bool hasDuplicitGoogleAccount);

        // inserts user
        int InsertUser(User user);

        // update user
        void UpdateUser(User user, HashSet<UserRoleTypeEnum> roleRestrictions, bool deleteLoginsWhenGoogleAccountIsChanged = false);

        // delete user
        void DeleteUser(int userId);

    }

}
    