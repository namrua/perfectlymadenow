using System.Collections.Generic;
using AutomationSystem.Main.Contract.Contacts.AppLogic.Models;
using AutomationSystem.Main.Model;
using ContactListItem = AutomationSystem.Main.Contract.Contacts.AppLogic.Models.ContactListItem;

namespace AutomationSystem.Main.Core.Contacts.AppLogic
{
    public interface IContactProvider
    {
        List<ContactListItem> GetContacts(ContactListFilter filter);

        List<ContactListItemDefinition> RemoveBlackListedItems(List<ContactListItemDefinition> contactListDefinitions);
    }
}
