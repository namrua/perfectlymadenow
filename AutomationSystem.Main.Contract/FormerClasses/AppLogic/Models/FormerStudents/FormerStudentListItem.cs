using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents
{
    public class FormerStudentListItem
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
        public FormerClassListItem Class { get; set; }


        public FormerStudentListItem()
        {
            Address = new AddressDetail();
            Class = new FormerClassListItem();
        }
    }
}
