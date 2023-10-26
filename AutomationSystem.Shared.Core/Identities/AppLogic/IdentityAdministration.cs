using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Base.Contract.Identities.Models;
using AutomationSystem.Shared.Contract.Identities.AppLogic;
using AutomationSystem.Shared.Contract.Identities.AppLogic.Models;
using AutomationSystem.Shared.Contract.Identities.Data;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Core.Identities.AppLogic
{
    /// <summary>
    /// Identity administration service
    /// </summary>
    public class IdentityAdministration : IIdentityAdministration
    {

        // private components
        private readonly IIdentityDatabaseLayer identityDb;
        private readonly IIdentityResolver identityResolver;

        // constructor
        public IdentityAdministration(IIdentityDatabaseLayer identityDb, IIdentityResolver identityResolver)
        {
            this.identityDb = identityDb;
            this.identityResolver = identityResolver;
        }


        // gets all users
        public List<UserListItem> GetUsers()
        {
            var users = identityDb.GetUsers();
            var result = users.Select(x => new UserListItem
            {
                UserId = x.UserId,
                Name = x.Name,
                GoogleAccount = x.GoogleAccount,
                Active = x.Active
            }).ToList();
            return result;
        }

        // get new user for edit
        public UserForEdit GetNewUserForEdit()
        {
            var result = InitializeUserForEdit(GetUserRoleTypeRestrictions());
            result.Form.Active = true;
            return result;
        }

        // get user for edit by id
        public UserForEdit GetUserForEdit(int userId)
        {
            var user = identityDb.GetUserById(userId);
            if (user == null)
                throw new ArgumentException($"There is no User with id {userId}");

            // creates result
            var result = InitializeUserForEdit(GetUserRoleTypeRestrictions());
            result.Form = new UserForm
            {
                UserId = userId,
                Name = user.Name,
                GoogleAccount =  user.GoogleAccount,
                Active = user.Active,
                UserRoles = user.UserRoleAssignments.Select(x => x.UserRoleTypeId).ToArray()                
            };
          
            return result;
        }

        // get user for edit based on form
        public UserForEdit GetFormUserForEdit(UserForm form, UserValidationResult validation)
        {           
            var result = InitializeUserForEdit(GetUserRoleTypeRestrictions());
            result.Form = form;
            if (validation.HasDuplicitGoogleAccount)
                result.ForbiddenGoogleAccount = form.GoogleAccount;
            if (validation.HasDuplicitName)
                result.ForbiddenName = form.Name;
            return result;
        }

        // validates user form
        public UserValidationResult ValidateUserForm(UserForm form)
        {
            identityDb.CheckUserDuplicities(form.UserId, form.Name, form.GoogleAccount, 
                out bool hasDuplicitName, out bool hasDuplicitGoogleAccount);
            var result = new UserValidationResult();
            result.HasDuplicitName = hasDuplicitName;
            result.HasDuplicitGoogleAccount = hasDuplicitGoogleAccount;
            result.IsValid = !hasDuplicitName && !hasDuplicitGoogleAccount;
            return result;
        }


        // saves user
        public long SaveUser(UserForm user)
        {
            var restrictions = GetUserRoleTypeRestrictions();
            var dbUser = ConvertToUser(user, restrictions);
            var result = user.UserId;
            if (user.UserId == 0)            
                result = identityDb.InsertUser(dbUser);
            else
                identityDb.UpdateUser(dbUser, restrictions, true);
            return result;
        }

        // deletes user
        public void DeleteUser(int userId)
        {
            identityDb.DeleteUser(userId);
        }


        #region

        // initializes user for edit
        // todo: move to convertor
        private UserForEdit InitializeUserForEdit(HashSet<UserRoleTypeEnum> userRoleTypeRestrictions)
        {
            var result = new UserForEdit();
            var allUserRoleTypes = identityDb.GetUserRoleTypes();
            result.RoleTypes = userRoleTypeRestrictions != null
                ? allUserRoleTypes.Where(x => userRoleTypeRestrictions.Contains(x.UserRoleTypeId)).ToList()
                : allUserRoleTypes;
            return result;
        }

        // converts user for to user 
        // todo: move to convertor
        private User ConvertToUser(UserForm user, HashSet<UserRoleTypeEnum> userRoleTypeRestrictions)
        {
            var result = new User();
            result.UserId = user.UserId;
            result.Name = user.Name;
            result.GoogleAccount = user.GoogleAccount;
            result.Active = user.Active;

            // adds roles
            foreach (var userRoleTypeId in user.UserRoles)
            {
                if (userRoleTypeRestrictions != null && !userRoleTypeRestrictions.Contains(userRoleTypeId))
                    continue;
                var userRoleAssignment = new UserRoleAssignment
                {
                    UserId = user.UserId,
                    UserRoleTypeId = userRoleTypeId,
                };
                result.UserRoleAssignments.Add(userRoleAssignment);
            }

            return result;
        }

        // gets user role type resutrictions - null = no restrictions
        private HashSet<UserRoleTypeEnum> GetUserRoleTypeRestrictions()
        {
            // full access
            if (identityResolver.IsEntitleGranted(Entitle.CoreUserAccounts))
                return null;

            // restricted access
            if (identityResolver.IsEntitleGranted(Entitle.CoreUserAccountsRestricted))
                return new HashSet<UserRoleTypeEnum>
                {
                    UserRoleTypeEnum.Coordinator,
                    UserRoleTypeEnum.DistanceCoordinator,
                    UserRoleTypeEnum.SuperCoordinator
                };

            // no access
            return new HashSet<UserRoleTypeEnum>();
        }

        #endregion

    }

}
