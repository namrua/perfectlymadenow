using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Persons.AppLogic.Models
{
    /// <summary>
    /// Person list item
    /// </summary>
    public class PersonListItem
    {

        // public properties
        public long PersonId { get; set; }

        [DisplayName("Name")]
        public string FullName { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Profile")]
        public string Profile { get; set; }

    }

}
