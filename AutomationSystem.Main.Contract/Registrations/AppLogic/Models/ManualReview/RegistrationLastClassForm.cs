using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Shared.Contract.Localisation.AppLogic;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview
{
    /// <summary>
    /// Registration last class form
    /// </summary>
    public class RegistrationLastClassForm
    {

        [HiddenInput]
        public long ClassRegistrationId { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("Location")]
        [LocalisedText("Metadata", "Location")]
        public string Location { get; set; }

        [Required]
        [DisplayName("Month")]
        [PickInputOptions(NoItemText = "no month", Placeholder = "select month")]
        [LocalisedText("Metadata", "Month")]
        [LocalisedText("Metadata", "MonthNoItemText", "NoItemText")]
        [LocalisedText("Metadata", "MonthPlaceholder", "Placeholder")]
        public int? Month { get; set; }

        [Required]
        [DisplayName("Year")]
        [PickInputOptions(NoItemText = "no year", Placeholder = "select year")]
        [LocalisedText("Metadata", "Year")]
        [LocalisedText("Metadata", "YearNoItemText", "NoItemText")]
        [LocalisedText("Metadata", "YearPlaceholder", "Placeholder")]
        public int? Year { get; set; }

    }

}
