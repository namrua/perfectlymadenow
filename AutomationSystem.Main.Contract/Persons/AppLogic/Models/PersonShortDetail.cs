using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Persons.AppLogic.Models
{
    /// <summary>
    /// Person detail
    /// </summary>
    public class PersonShortDetail
    {
        [DisplayName("ID")]
        public long PersonId { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Roles")]
        public List<string> Roles { get; set; }


        [DisplayName("Name")]
        public string Name { get; set; }


        public bool CanDelete { get; set; }


        // constructor
        public PersonShortDetail()
        {
            Roles = new List<string>();
        }
    }
}