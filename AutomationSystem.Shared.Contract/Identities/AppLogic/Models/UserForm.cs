using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Shared.Contract.Identities.AppLogic.Models
{
    /// <summary>
    /// User form
    /// </summary>
    public class UserForm
    {

        [HiddenInput]
        public int UserId { get; set; }

        [Required]
        [MaxLength(64)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(64)]
        [DisplayName("Google account")]
        public string GoogleAccount { get; set; }

        [DisplayName("Is active")]
        public bool Active { get; set; }

        [DisplayName("Is assigned")]
        public UserRoleTypeEnum[] UserRoles { get; set; }

        // constructor
        public UserForm()
        {
            UserRoles = new UserRoleTypeEnum[0];
        }

    }
}
