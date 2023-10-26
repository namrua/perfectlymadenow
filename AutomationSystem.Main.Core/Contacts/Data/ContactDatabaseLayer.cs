using System;
using AutomationSystem.Main.Model;
using AutomationSystem.Main.Model.Queries;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Main.Core.Contacts.Data.Extensions;
using AutomationSystem.Main.Core.Contacts.Data.Models;

namespace AutomationSystem.Main.Core.Contacts.Data
{
    public class ContactDatabaseLayer : IContactDatabaseLayer
    {
        #region ContactList operations

        public ContactList GetContactListById(long contactListId, ContactListIncludes includes = ContactListIncludes.None)
        {
            using (var context = new MainEntities())
            {
                var result = context.ContactLists.AddIncludes(includes).Active().FirstOrDefault(x => x.ContactListId == contactListId);
                return result;
            }
        }

        public long InsertContactList(ContactList list)
        {
            using (var context = new MainEntities())
            {
                var result = context.ContactLists.Add(list);
                context.SaveChanges();
                return result.ContactListId;
            }
        }

        public void SetContactListSenderId(long contactListId, long? senderId)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ContactLists.Active().FirstOrDefault(x => x.ContactListId == contactListId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no contact list with id {contactListId}.");
                }

                toUpdate.SenderId = senderId;
                context.SaveChanges();
            }
        }

        public void SetContactListAsNotified(long contactListId)
        {
            using (var context = new MainEntities())
            {
                var toUpdate = context.ContactLists.Active().FirstOrDefault(x => x.ContactListId == contactListId);
                if (toUpdate == null)
                {
                    throw new ArgumentException($"There is no contact list with id {contactListId}.");
                }

                toUpdate.IsNotified = true;
                toUpdate.Notified = DateTime.Now;
                context.SaveChanges();
            }
        }

        #endregion

        #region BlackList operations

        public List<ContactBlackList> GetBlackLists()
        {
            using (var context = new MainEntities())
            {
                var result = context.ContactBlackLists.Active().ToList();
                return result;
            }
        }

        public void AddContactToBlackList(ContactBlackList contact)
        {
            using (var context = new MainEntities())
            {
                if (context.ContactBlackLists.Active().Any(x => x.Email == contact.Email))
                {
                    return;
                }

                context.ContactBlackLists.Add(contact);
                context.SaveChanges();
            }
        }
        
        public void RemoveContactFromBlackList(string email)
        {
            using (var context = new MainEntities())
            {
                var toDelete = context.ContactBlackLists.Active().Where(x => x.Email == email).ToList();
                context.ContactBlackLists.RemoveRange(toDelete);
                context.SaveChanges();
            }
        }

        #endregion
    }
}
