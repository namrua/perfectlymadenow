using AutomationSystem.Main.Model;
using System.Collections.Generic;
using AutomationSystem.Main.Core.Contacts.Data.Models;

namespace AutomationSystem.Main.Core.Contacts.Data
{
    public interface IContactDatabaseLayer
    {
        #region ContactList operations

        ContactList GetContactListById(long contactListId, ContactListIncludes includes = ContactListIncludes.None);

        long InsertContactList(ContactList list);

        void SetContactListSenderId(long contactListId, long? senderId);

        void SetContactListAsNotified(long contactListId);

        #endregion

        #region BlackList operations

        List<ContactBlackList> GetBlackLists();

        void AddContactToBlackList(ContactBlackList contact);

        void RemoveContactFromBlackList(string email);

        #endregion

        
    }
}
