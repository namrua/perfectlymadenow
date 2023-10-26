using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Preferences
{
    /// <summary>
    /// Class preference form
    /// </summary>
    public class ClassPreferenceForm
    {

        [HiddenInput]
        public long ProfileId { get; set; }

        [MaxLength(256)]
        [DisplayName("Detached Homepage")]
        public string HomepageUrl { get; set; }

        [Required]
        [DisplayName("Color scheme of registration pages")]
        [PickInputOptions(NoItemText = "no scheme", Placeholder = "select scheme")]
        public RegistrationColorSchemeEnum RegistrationColorSchemeId { get; set; }

        [Required]
        [DisplayName("Currency")]
        [PickInputOptions]
        public CurrencyEnum CurrencyId { get; set; }

        [HiddenInput]
        public long? OriginHeaderPictureId { get; set; }

        [DisplayName("Remove current picture")]
        public bool RemoveHeaderPicture { get; set; } = false;

        [DisplayName("Send certificates by Thank you email")]
        public bool SendCertificatesByEmail { get; set; }

        [MaxLength(64)]
        [DisplayName("Venue name")]
        public string VenueName { get; set; }

        [Required]
        [MaxLength(16)]
        [DisplayName("CRF Location code")]
        public string LocationCode { get; set; }

        [DisplayName("Location info")]
        [PickInputOptions(ControlType = PickControlType.TypeaheadDropDownInput, Placeholder = "select location info")]
        public long? LocationInfoId { get; set; }

    }
}
