using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Contacts.AppLogic.Models
{
    public class ContactListItem
    {
        public long FormerStudentId { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
        
        public bool IsSelected { get; set; }
        
        public bool IsDisabled { get; set; }
    }
}
