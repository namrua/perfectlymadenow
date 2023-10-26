using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Persons.AppLogic.Models
{
    /// <summary>
    /// Person form
    /// </summary>
    public class PersonForm
    {

        [HiddenInput]
        public long PersonId { get; set; }
        [HiddenInput]
        public int? AssignedUserId { get; set; }
        [HiddenInput]
        public long? ProfileId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        [DisplayName("Phone number")]
        public string Phone { get; set; }

        [DisplayName("Coordinator #")]
        [Range(0, 1000000)]
        [SpinnerInputOptions(Min = 0, Max = 1000000)]
        public int? CoordinatorNumber { get; set; }

        [DisplayName("Person's address")]
        public AddressForm Address { get; set; }

        [DisplayName("Is assigned")]
        public PersonRoleTypeEnum[] PersonRoles { get; set; }

        [DisplayName("Is default")]
        public PersonRoleTypeEnum[] DefaultPersonRoles { get; set; }

        // constructor
        public PersonForm()
        {
            PersonRoles = new PersonRoleTypeEnum[0];
            DefaultPersonRoles = new PersonRoleTypeEnum[0];
            Address = new AddressForm();
        }

    }

}
