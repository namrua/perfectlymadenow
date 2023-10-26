using AutomationSystem.Main.Core.Contacts.Data.Models;
using AutomationSystem.Main.Model;
using System.Data.Entity.Infrastructure;

namespace AutomationSystem.Main.Core.Contacts.Data.Extensions
{
    public static class ContactIncludeExtensions
    {
        public static DbQuery<ContactList> AddIncludes(this DbQuery<ContactList> query, ContactListIncludes includes)
        {
            if (includes.HasFlag(ContactListIncludes.ContactListItems))
            {
                query = query.Include("ContactListItems");
            }

            if (includes.HasFlag(ContactListIncludes.ContactListItemsFormerStudent))
            {
                query = query.Include("ContactListItems.FormerStudent");
            }

            if (includes.HasFlag(ContactListIncludes.ContactListItemsFormerStudentAddress))
            {
                query = query.Include("ContactListItems.FormerStudent.Address");
            }

            return query;
        }
    }
}
