using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Shared.Contract.Identities.AppLogic.Models
{
    /// <summary>
    /// User list item
    /// </summary>
    public class UserListItem
    {

        // public properties
        public int UserId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [EmailAddress]
        [DisplayName("Google account")]
        public string GoogleAccount { get; set; }

        [DisplayName("Is active")]
        public bool Active { get; set; }

    }
}
