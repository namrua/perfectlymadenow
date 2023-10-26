using System;
using AutomationSystem.Shared.Contract.Emails.AppLogic.Models;
using System.Collections.Generic;
using System.ComponentModel;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Contacts.AppLogic.Models
{
    public class ContactListDetail
    {
        public long ContactListId { get; set; }

        public bool IsNotified { get; set; }

        [DisplayName("Notified")]
        public DateTime? Notified { get; set; }

        public string SenderName { get; set; }
        public List<PickerItem> Senders { get; set; } = new List<PickerItem>();
        public ContactListSenderForm Form { get; set; } = new ContactListSenderForm();

        [DisplayName("Contacts")]
        public List<string> Emails { get; set; }

        public List<EmailTemplateListItem> EmailTemplates { get; set; } = new List<EmailTemplateListItem>();
    }
}
