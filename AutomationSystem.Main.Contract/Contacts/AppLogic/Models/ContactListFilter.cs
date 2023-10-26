using CorabeuControl.ModelMetadata;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomationSystem.Main.Contract.Contacts.AppLogic.Models
{
    public class ContactListFilter
    {
        [Required]
        [DisplayName("Profile")]
        [PickInputOptions(Placeholder = "select profile")]
        public long ProfileId { get; set; }

        [DisplayName("Include disabled contacts")]
        public bool IncludeDisabledContacts { get; set; }
    }
}
