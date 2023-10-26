using System.ComponentModel;
using System.Web.Mvc;
using AutomationSystem.Main.Contract.Addresses.AppLogic.Models;
using AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerStudents
{
    /// <summary>
    /// Former student filter
    /// </summary>
    [Bind(Include = "FormerClassId, Class, Address, Email, Phone, Contact")]
    public class FormerStudentFilter
    {

        public long? FormerClassId { get; set; }

        [DisplayName("Class")]
        public FormerClassFilter Class { get; set; }

        [DisplayName("Address")]
        public AddressFilter Address { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Contact")]
        public string Contact { get; set; }

        // constructor
        public FormerStudentFilter()
        {
            Class = new FormerClassFilter();
            Address = new AddressFilter();
        }

    }

}
