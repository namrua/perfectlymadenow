using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.Contacts.AppLogic.Models
{
    public class ContactListDefinition
    {
        public long ProfileId { get; set; }

        public List<ContactListItemDefinition> ContactListItems { get; set; } = new List<ContactListItemDefinition>();
    }
}
