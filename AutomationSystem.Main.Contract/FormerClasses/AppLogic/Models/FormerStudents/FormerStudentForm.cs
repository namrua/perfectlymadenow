using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents
{
    /// <summary>
    /// Former student form
    /// </summary>
    public class FormerStudentForm
    {

        [HiddenInput]
        public long FormerStudentId { get; set; }
        [HiddenInput]
        public long FormerClassId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [MaxLength(15)]
        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Address")]
        public AddressForm Address { get; set; }

        // constructor
        public FormerStudentForm()
        {
            Address = new AddressForm();
        }

    }

}
