using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Preferences.AppLogic.Models
{
    /// <summary>
    /// Izi preferences form
    /// </summary>
    public class IziPreferenceForm
    {

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        [DisplayName("Master coordinator email")]
        public string MasterCoordinatorRecipient { get; set; }

        [DisplayName("Master lead instructor")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput, Placeholder = "select master lead instructor")]
        public long? MasterLeadInstructorId { get; set; }

    }

}
