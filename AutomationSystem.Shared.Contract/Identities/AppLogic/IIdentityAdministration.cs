using System.Collections.Generic;
using AutomationSystem.Shared.Contract.Identities.AppLogic.Models;

namespace AutomationSystem.Shared.Contract.Identities.AppLogic
{
    /// <summary>
    /// Identity administration service
    /// </summary>
    public interface IIdentityAdministration
    {

        // gets all users
        List<UserListItem> GetUsers();

        // get new user for edit
        UserForEdit GetNewUserForEdit();

        // get user for edit by id
        UserForEdit GetUserForEdit(int userId);

        // get user for edit based on form
        UserForEdit GetFormUserForEdit(UserForm form, UserValidationResult validation);

        // validates user form
        UserValidationResult ValidateUserForm(UserForm form);


        // saves user
        long SaveUser(UserForm user);

        // deletes user
        void DeleteUser(int userId);

    }

}
