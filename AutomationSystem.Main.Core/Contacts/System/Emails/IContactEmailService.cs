using System.Collections.Generic;
using AutomationSystem.Main.Model;

namespace AutomationSystem.Main.Core.Contacts.System.Emails
{
    public interface IContactEmailService
    {
        List<long> SendContactListEmails(ContactList contactList);
    }
}
