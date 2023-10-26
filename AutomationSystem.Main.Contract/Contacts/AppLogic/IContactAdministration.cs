using AutomationSystem.Main.Contract.Contacts.AppLogic.Models;

namespace AutomationSystem.Main.Contract.Contacts.AppLogic
{
    public interface IContactAdministration
    {
        ContactListPageModel GetContactListPageModel(ContactListFilter filter, bool search);

        void AddContactToBlackList(string email);

        void RemoveContactFromBlackList(string email);

        long CreateContactList(ContactListDefinition contactListDefinition);

        ContactListDetail GetContactListDetail(long contactListId);

        void UpdateSenderId(ContactListSenderForm contactListSenderForm);

        void NotifyContacts(long contactListId);
    }
}
