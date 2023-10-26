using System.Collections.Generic;
using AutomationSystem.Shared.Model;

namespace AutomationSystem.Shared.Contract.Identities.AppLogic.Models
{
    /// <summary>
    /// User for edit
    /// </summary>
    public class UserForEdit
    {

        // public properties
        public List<UserRoleType> RoleTypes { get; set; }
        public UserForm Form { get; set; }
        public string ForbiddenName { get; set; }
        public string ForbiddenGoogleAccount { get; set; }

        // constructor
        public UserForEdit()
        {
            RoleTypes = new List<UserRoleType>();
            Form = new UserForm();
        }

    }

}
