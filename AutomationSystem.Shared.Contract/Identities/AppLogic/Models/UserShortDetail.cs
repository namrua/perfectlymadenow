using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Shared.Contract.Identities.AppLogic.Models
{
    /// <summary>
    /// Determines fundamental properties of user
    /// </summary>
    public class UserShortDetail 
    {

        [DisplayName("ID")]
        public int UserId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Is active")]
        public bool Active { get; set; }

    }
}
