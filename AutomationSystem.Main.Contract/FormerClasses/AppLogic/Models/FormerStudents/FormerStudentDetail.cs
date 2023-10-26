using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents
{
    /// <summary>
    /// Former student detail
    /// </summary>
    public class FormerStudentDetail
    {

        [DisplayName("ID")]
        public long FormerStudentId { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Address")]
        public AddressDetail Address { get; set; }

        [DisplayName("Class")]
        public FormerClassDetail Class { get; set; }

        // access levels
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        // constructor
        public FormerStudentDetail()
        {
            Address = new AddressDetail();
            Class = new FormerClassDetail();
        }
    }

}
