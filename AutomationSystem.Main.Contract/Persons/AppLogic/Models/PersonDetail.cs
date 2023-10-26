using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Persons.AppLogic.Models
{
    /// <summary>
    /// Person detail
    /// </summary>
    public class PersonDetail
    {

        [DisplayName("ID")]
        public long PersonId { get; set; }

        [DisplayName("Profile ID")]
        public long? ProfileId { get; set; }

        [DisplayName("Profile")]
        public string Profile { get; set; }

        [DisplayName("Address")]
        public AddressDetail Address { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Coordinator #")]
        public int? CoordinatorNumber { get; set; }


        // access rights
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }


        [DisplayName("Assigned roles")]
        public List<PersonRoleDetail> AssignedRoles { get; set; } = new List<PersonRoleDetail>();

    }

}
