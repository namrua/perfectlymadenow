using CorabeuControl.Components;
using System.Collections.Generic;

namespace AutomationSystem.Main.Contract.Contacts.AppLogic.Models
{
    public class ContactListPageModel
    {
        public List<ContactListItem> Items { get; set; } = new List<ContactListItem>();

        public List<DropDownItem> Profiles { get; set; } = new List<DropDownItem>();

        public ContactListFilter Filter { get; set; }

        public bool WasSearched { get; set; }

        public ContactListPageModel(ContactListFilter filter = null)
        {
            Filter = filter ?? new ContactListFilter();
        }
    }
}
