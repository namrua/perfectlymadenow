using CorabeuControl.ModelMetadata;
using System.ComponentModel;

namespace AutomationSystem.Main.Contract.Contacts.AppLogic.Models
{
    public class ContactListSenderForm
    {
        public long ContactListId { get; set; }

        [DisplayName("Sender")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput, Placeholder = "search sender")]
        public long? SenderId { get; set; }
    }
}
