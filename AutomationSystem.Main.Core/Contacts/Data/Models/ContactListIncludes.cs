using System;

namespace AutomationSystem.Main.Core.Contacts.Data.Models
{
    [Flags]
    public enum ContactListIncludes
    {
        None = 0,
        ContactListItems = 1 << 0,
        ContactListItemsFormerStudent = 1 << 1,
        ContactListItemsFormerStudentAddress = 1 << 2
    }
}
